import { useState, useEffect, useRef } from 'react';
import { AuthContext } from '../context/AuthContext';
import Navbar from './Navbar';
import PopUpModal from '../assets/PopUpModal';


export default function Collections() {

    const [collectionsModalOpen, setCollectionsModalOpen] = useState(false);
    const [popUpMessage, setPopUpMessage] = useState(null);
    const [collectionList, setCollectionList] = useState([]);
    const didRun = useRef(false);

    useEffect(() => {

        async function loadCollections() {

            try {
                const resp = await fetch("https://localhost:7207/api/Collection/getcollections", {
                    method: 'POST',
                    credentials: 'include'
                });

                if (!resp.ok) {
                    setPopUpMessage("Failed to load user collections");
                    setCollectionsModalOpen(true);
                    return;
                }

                const data = await resp.json();
                setCollectionList(data);

            } catch (err) {
                console.log(`Error while fetching collections: ${err}`);
            }

        }

        if (didRun.current) return;
        didRun.current = true;

        loadCollections();

    }, [])

    useEffect(() => {
        console.log(`Updated collectionList; Current number of items: ${collectionList.length}`);
    }, [collectionList])

    return (
        <>
        <Navbar />

            {collectionList.map((item, index) => (
                <div key={index}>
                    <div style={{ display: 'flex', gap: '5px', borderTop: '2px solid gray', borderBottom: '2px solid gray'}}>
                        <h2 style={{color: 'gray'}}>+</h2>
                        <h2>{item.collectionName}</h2>
                    </div>
                </div>
            ))}
            <PopUpModal isOpen={collectionsModalOpen} onClose={() => { setCollectionsModalOpen(false); setPopUpMessage(null); }}>
                <p>{popUpMessage}</p>
            </PopUpModal>
        </>
    );

}
