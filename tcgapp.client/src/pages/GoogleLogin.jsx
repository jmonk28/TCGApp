import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';


export default function GoogleLogin() {
    const navigate = useNavigate();

    function googleRedirect() {
        var redirectUri = `accounts.google.com/o/oauth2/v2/auth?
                           client_id=&
                           include_granted_scopes=true&
                           response_type=code&
                           redirect_uri=&
                           `
        
    }
}