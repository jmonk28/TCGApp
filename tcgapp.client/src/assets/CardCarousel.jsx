import { useState, useEffect } from 'react';
import ReactSimplyCarousel from 'react-simply-carousel';



export default function CardCarousel({ cards, container, cardClass }) {

    const [activeSlideIndex, setActiveSlideIndex] = useState(0);

    useEffect(() => {
        // Force carousel to recalc width after mount
        setTimeout(() => {
            window.dispatchEvent(new Event('resize'));
        }, 0);
    }, []);

    return (
        <div>
            <ReactSimplyCarousel
                activeSlideIndex={activeSlideIndex}
                onRequestChange={setActiveSlideIndex}
                itemsToShow={1}
                itemsToScroll={1}
                hideNavIfAllVisible={false}
                forwardBtnProps={{ style: { backgroundColor: '#00B3B8', margin: '15px' }, children: <span>{'→'}</span> }}
                backwardBtnProps={{ style: { backgroundColor: '#00B3B8', margin: '15px' }, children: <span>{'←'}</span> }}
                speed={200}
                easing="linear"
            >
                {cards.map((card, index) => (
                    <div key={index}>
                        <div className={container}>
                            <img src={card.image} className={cardClass} />
                        </div>
                        <h5>{card.name}</h5>
                    </div>
                ))}
            </ReactSimplyCarousel>
        </div>
    );


}
