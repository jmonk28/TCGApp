import { useState, useEffect, useRef, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
import Navbar from './Navbar';
import PopUpModal from '../assets/PopUpModal';


export default function Collections() {

    const [collectionsModalOpen, setCollectionsModalOpen] = useState(false);
    const [newCollectionsModalOpen, setNewCollectionsModalOpen] = useState(false);
    const [popUpMessage, setPopUpMessage] = useState(null);
    const [collectionList, setCollectionList] = useState([]);
    const [newCollectionName, setNewCollectionName] = useState(null);
    const [newCollectionType, setNewCollectionType] = useState("Misc");
    const [typeDropDownOpen, setTypeDropDownOpen] = useState(false);
    const COLLECTION_TYPES = ["Misc", "Magic", "Pokemon", "Yu-Gi-Oh", "One Piece"];
    const didRun = useRef(false);
    const { user } = useContext(AuthContext);
    const navigate = useNavigate();

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


    async function submitCollection(e) {

        e.preventDefault();

        try {
            const resp = await fetch("https://localhost:7207/api/Collection/addcollection", {
                method: 'POST',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ collectionName: newCollectionName, collectionType: newCollectionType })
            });

            if (!resp.ok) {
                setPopUpMessage("Failed to add collection");
                setCollectionsModalOpen(true);
                return;
            }
        } catch (err) {
            console.log(`Error while adding collection: ${err}`);
        }

    }

    return (
        <>
            <Navbar />

            <h1>{user}'s Collections</h1>

            {collectionList.map((item, index) => (
                <div key={index}>
                    <div style={{ display: 'flex', borderBottom: '2px solid gray', cursor: 'pointer' }} onClick={() => {navigate(`/collectionview/${user}/${item.collectionID}`)}}>
                        <h2>{item.collectionName}</h2>
                        <div style={{ marginLeft: 'auto' }}><h2 style={{ color: 'gray' }}>→</h2></div>
                    </div>
                </div>
            ))}
            <div style={{ justifyContent: 'center', marginTop: '12px' }}><button onClick={() => { setNewCollectionsModalOpen(true) }}>Add New Collection</button></div>
            <PopUpModal isOpen={collectionsModalOpen} onClose={() => { setCollectionsModalOpen(false); setPopUpMessage(null); }}>
                <p>{popUpMessage}</p>
            </PopUpModal>
            <PopUpModal isOpen={newCollectionsModalOpen} onClose={() => { setNewCollectionsModalOpen(false); }}>
                <form onSubmit={submitCollection} style={{display: 'block', gap: '10px'}} noValidate>
                    <label htmlFor="collectionName">Collection Name:</label>
                    <input id="collectionName" type="text" placeholder="NewCollection123" style={{display: 'block', width: '100%', padding: 'auto'}} onChange={e => setNewCollectionName(e.target.value)} />
                    <div style={{marginTop: '12px'}}>
                        <label htmlFor="collectionType">Collection Type:</label>
                        <div id="collectionType" style={{ cursor: 'pointer', border: '2px solid gray', padding: 8 }} onClick={() => setTypeDropDownOpen(true)}>{newCollectionType}</div>
                        <div style={{ position: 'absolute' }} hidden={!typeDropDownOpen}>
                            {COLLECTION_TYPES.map((item, index) => (
                                <div key={index} style={{ padding: '15px', cursor: 'pointer' }} onClick={() => { setNewCollectionType(item); setTypeDropDownOpen(false); }}>{item}</div>
                            ))}
                        </div>
                    </div>
                    <button type="submit" style={{marginTop: '15px'}}>Add Collection</button>
                </form>
            </PopUpModal>
        </>
    );

}
