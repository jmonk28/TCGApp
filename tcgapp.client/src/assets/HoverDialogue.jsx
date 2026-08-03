

export default function HoverDialogue({ dialogueStyle, message }) {

    return (
        <>
        <div style={dialogueStyle}>
            {message}
        </div>
        </>
    );

}