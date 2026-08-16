import { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { useParams } from 'react-router-dom';
import Navbar from './Navbar';
import PopUpModal from '../assets/PopUpModal';

export default function CollectionView() {

    const { collectionid } = useParams();
    const [infoModalOpen, setInfoModalOpen] = useState(false);
    const [popUpMessage, setPopUpMessage] = useState(null);
    const [trueCardList, setTrueCardList] = useState([]);
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
                console.log(data[0]);

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
        <Navbar />
        <main className="collectionviewmain" style={{padding: '100px'}}>
            {trueCardList.length == 0 && (
                <p>No cards in this collection yet</p>
            )}
            {trueCardList.length > 0 && (trueCardList.map((card, index) => (
                <div key={index} style={{display: 'flex', gap: '10px'}}>
                    <div className="card-display">
                        <img src={card.image} className="card-image" />
                    </div>
                    <h5>{card.cardName}</h5>
                </div>
            )))}
            <div style={{ justifyContent: 'center', marginTop: '12px' }}><button>Add Cards</button></div>
        </main>
        <PopUpModal isOpen={infoModalOpen} onClose={() => { setInfoModalOpen(false); setPopUpMessage(null); navigate("/"); }}>
            <p>{popUpMessage}</p>
        </PopUpModal>
    </>
    );
}