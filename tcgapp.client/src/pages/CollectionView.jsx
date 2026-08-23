import { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { useParams } from 'react-router-dom';
import Navbar from './Navbar';
import PopUpModal from '../assets/PopUpModal';
import CardCarousel from '../assets/CardCarousel';

export default function CollectionView() {

    const { collectionid } = useParams();
    const [infoModalOpen, setInfoModalOpen] = useState(false);
    const [addCardModalOpen, setAddCardModalOpen] = useState(false);
    const [popUpMessage, setPopUpMessage] = useState(null);
    const [trueCardList, setTrueCardList] = useState([]);
    const [databaseCardList, setDatabaseCardList] = useState([]);
    const [cardsToAdd, setCardsToAdd] = useState([]);
    //Define structure for keeping track of each card's desired count
    const [counts, setCounts] = useState({})
    const didRun = useRef(false);
    const navigate = useNavigate();

    useEffect(() => {

        async function loadCards() {

            //Fetch Card instances associated with Collection's CollectionCards

            try {
                const resp = await fetch("https://localhost:7207/api/Card/getdatabasecardsfromcollectioncards", {
                    method: 'POST',
                    credentials: 'include',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(collectionid)
                });

                if (!resp.ok) {
                    setPopUpMessage("Failed to load collection cards");
                    setInfoModalOpen(true);
                    return;
                }

                const data = await resp.json();
                setTrueCardList(data);

            } catch (err) {
                console.log(`Error while fetching collection cards: ${err}`);
            }

        }

        if (didRun.current) return;
        didRun.current = true;

        loadCards();

    }, [])

    useEffect(() => {
        console.log("cardsToAdd updated:", cardsToAdd);
    }, [cardsToAdd]);


    async function pullDBCards() {

        try {
            const resp = await fetch("https://localhost:7207/api/Card/getdatabasecards", {
                method: 'POST',
                credentials: 'include'
            });

            if (!resp.ok) {
                setPopUpMessage("Failed to fetch cards from database; try again later");
                setInfoModalOpen(true);
                setAddCardModalOpen(false);
                return;
            }

            const data = await resp.json();
            setDatabaseCardList(data);
        } catch (err) {
            console.log(`Error while fetching database cards: ${err}`);
        }

    }

    async function cardSubmit(e) {
        //Prevent page reload
        e.preventDefault();

        //Place the requested counts of each card into each card object and return it as a new list
        const newCardList = databaseCardList.map(card => ({
            ...card,
            cardCount: counts[card.cardID] ?? 0
        })).filter(card => card.cardCount > 0);

        setCardsToAdd(newCardList);

        //Sanity check that objects are what I expect
        console.log(cardsToAdd[0]);

        try {
            const resp = await fetch("https://localhost:7207/api/Card/addcardstocollection", {
                method: 'POST',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({collectionID: collectionid, cards: newCardList})
            })

            if (!resp.ok) {
                setPopUpMessage("Failed to add requested cards");
                setInfoModalOpen(true);
                setAddCardModalOpen(false);
                return;
            }

        } catch (err) {
            console.log(`Error while submitting collection cards: ${err}`);
        }

        setAddCardModalOpen(false);

    }

    return (
    <>
        <Navbar />
        <main className="collectionviewmain" style={{padding: '100px'}}>
            {trueCardList.length == 0 && (
                <p>No cards in this collection yet</p>
            )}
            <div style={{ display: 'flex', gap: '10px', width: 'auto', justifyContent: 'center'}}>
                {trueCardList.length > 0 && (trueCardList.map((card, index) => (
                    <div key={index}>
                        <div style={{display: 'flex', gap: '5px'}}>
                            <div style={{ color: 'gray', fontSize: '25px' }}>x{card.cardCount}</div>
                            <div className="collection-card-display">
                                <img src={card.image} className="collection-card-image" />
                            </div>
                        </div>
                        <h5>{card.cardName}</h5>
                        <h6 style={{ color: '#00B3B8' }}>{card.cardGame}</h6>
                    </div>
                )))}
            </div>
            <div style={{ justifyContent: 'center', marginTop: '12px' }}><button onClick={() => { pullDBCards(); setAddCardModalOpen(true); }}>Add Cards</button></div>
        </main>
        <PopUpModal isOpen={infoModalOpen} onClose={() => { setInfoModalOpen(false); setPopUpMessage(null); }}>
            <p>{popUpMessage}</p>
            </PopUpModal>
        <PopUpModal isOpen={addCardModalOpen} onClose={() => { setAddCardModalOpen(false); }}>
            <form onSubmit={cardSubmit} noValidate>
                <div style={{ justifyContent: 'center' }}>
                    <h2>Add Cards to Collection</h2>
                    <h4>Available Cards</h4>
                        {addCardModalOpen && (<CardCarousel cards={databaseCardList} container="collection-card-display" cardClass="collection-card-image" numItemsShow={4} numItemsScroll={4} selectOn={true} counts={counts} setCounts={setCounts} />)}
                </div>
                <button type="submit" style={{ padding: '10px 16px', margin: '10px' }}>
                    Add Cards
                </button>
            </form>
        </PopUpModal>
    </>
    );
}