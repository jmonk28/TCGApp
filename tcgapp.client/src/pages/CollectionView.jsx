import { useState, useEffect, useRef } from 'react';
import { useParams } from 'react-router-dom';


export default function CollectionView() {

    const { collectionid } = useParams();
    const [infoModalOpen, setInfoModalOpen] = useState(false);
    const [popUpMessage, setPopUpMessage] = useState(null);
    const [collectionCardList, setCollectionCardList] = useState([]);
    const [trueCardList, setTrueCardList] = useState([]);
    const didRun = useRef(false);

    useEffect(() => {

        async function loadCards() {

            //First we load the CollectionCards, all instances of all cards in the collection

            try {
                const resp = await fetch("https://localhost:7207/api/Card/getcollectioncards", {
                    method: 'POST',
                    credentials: 'include',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ collectionID: collectionid })
                });

                if (!resp.ok) {
                    setPopUpMessage("Failed to load collection cards");
                    setInfoModalOpen(true);
                    return;
                }

                const data = await resp.json();
                setCollectionCardList(data);

            } catch (err) {
                console.log(`Error while fetching collection cards: ${err}`);
            }

            //Then, we fetch the actual Card instances associated with those CollectionCards for display purposes

            try {
                const resp = await fetch("https://localhost:7207/api/Card/getdatabasecardsfromcollectioncards", {
                    method: 'POST',
                    credentials: 'include',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ cardList: collectionCardList })
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

    return (
    <>

        {collectionCardList.length == 0 && (
            <p>No cards in this collection yet</p>
        )}
        {trueCardList.length > 0 && (trueCardList.map((card, index) => (
            <div key={index}>
                <div className="card-display">
                    <img src={card.image} className="card-image" />
                </div>
                <h5>{card.cardName}</h5>
            </div>
        )))}
        <PopUpModal isOpen={infoModalOpen} onClose={() => { setInfoModalOpen(false); setPopUpMessage(null); }}>
            <p>{popUpMessage}</p>
        </PopUpModal>
    </>
    );
}