import { useRef, useEffect } from "react";
export default function PopUpModal({ isOpen, onClose, hasCloseBtn = true, children }) {

    const modalRef = useRef(null);

    useEffect(() => {
        const modalEl = modalRef.current;
        if (!modalEl) return;
        isOpen ? modalEl.showModal() : modalEl.close();
    }, [isOpen]);

    const handleKeyDown = (e) => {
        if (e.key === "Escape") onClose();
    };

    return (
        <dialog ref={modalRef} className="modal" onKeyDown={handleKeyDown}>
            {hasCloseBtn && (
                <button className="modal-close-btn" onClick={onClose} aria-label="Close">
                    X
                </button>
            )}
            {children}
        </dialog>
    );
}