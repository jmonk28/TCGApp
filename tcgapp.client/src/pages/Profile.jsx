import { useState, useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
import Navbar from "./Navbar";
import PopUpModal from '../assets/PopUpModal';

export default function Profile() {

    const { isLoggedIn } = useContext(AuthContext);
    const navigate = useNavigate();
    const [profileModalOpen, setProfileModalOpen] = useState(false);
    const [popUpMessage, setPopUpMessage] = useState(null);
    const [username, setUsername] = useState(null);
    const [email, setEmail] = useState(null);

    useEffect(() => {

        async function loadInformation() {

            if (!isLoggedIn) navigate("/login");

            //Pull user information via a call to the userinfo endpoint in our backend
            try {
                const resp = await fetch("https://localhost:7207/api/UserInformation/userinfo", {
                    method: 'POST',
                    credentials: 'include'
                });

                if (!resp.ok) {
                    setPopUpMessage("Failed to load user information, please try again");
                    setProfileModalOpen(true);
                    return;
                }

                const data = await resp.json();
                setUsername(data.username);
                setEmail(data.email);

            } catch (err) {
                console.log(err);
            }

        }

        loadInformation();

    }, [])

    return (
        <>
        <Navbar />
        <div style={{display: 'flex', marginTop: '20px'}}>
            <div style={{maxHeight: '200px', maxWidth: '200px', float: 'left'}}>
                <img src="/blank_profile_pic.png" style={{height: '200px', width: '200px', borderRadius: '5px'}} />
            </div>
            <div style={{ width: 'auto', marginLeft: '20px'}}>
                <div style={{ width: '200px', height: 'auto', border: '5px', borderColor: 'white' }}>
                    <h2>Username: {username}</h2>
                </div>
                <div style={{ width: '200px', height: 'auto', border: '5px', borderColor: 'white' }}>
                    <h2>Email: {email}</h2>
                </div>
            </div>

        
            <PopUpModal isOpen={profileModalOpen} onClose={() => { setProfileModalOpen(false); setPopUpMessage(null); }}>
                <p>{popUpMessage}</p>
            </PopUpModal>
        </div>
        </>
    );

}