import { useState, useEffect, useRef } from "react";
import { AuthContext } from "./AuthContext";


export function AuthProvider({ children }) {
    const [accessToken, setAccessToken] = useState(null);
    const [user, setUser] = useState(null);
    const didRun = useRef(false);

    useEffect(() => {

        async function grabToken() {
            try {
                const resp = await fetch("https://localhost:7207/api/Login/refresh", {
                    method: 'POST',
                    credentials: 'include'
                });

                if (resp.ok) {
                    const data = await resp.json();
                    setAccessToken(data.accessToken);
                    setUser(data.username);
                }
                else {
                    console.error("401 Unauthorized: unable to fetch refresh token");
                }
            } catch (err) {
                console.error("Could not refresh token: " + err);
            }
        }

        //Set timer to ensure browser has enough time to attach cookies before requesting refresh token (50ms)
        const timer = setTimeout(() => {
            if (didRun.current) return;
            didRun.current = true;

            grabToken();
        }, 50);

        return () => clearTimeout(timer);
    }, []);

    return (
        <AuthContext.Provider value={{
            accessToken,
            setAccessToken,
            user,
            setUser
        }}>
            {children}
        </AuthContext.Provider>
    );
}