import { useState } from 'react';


export default function HoverDialogue({ dialogueStyle, message }) {

    const [isShowing, setIsShowing] = useState(false);

    return (
        <>
        <div style={dialogueStyle}>
            {message}
        </div>
        </>
    );

}