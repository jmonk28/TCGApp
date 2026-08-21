import { useState, useEffect } from 'react';
import ReactSimplyCarousel from 'react-simply-carousel';



export default function CardCarousel({ cards, container, cardClass, numItemsShow, numItemsScroll, selectOn, counts, setCounts }) {

    const [activeSlideIndex, setActiveSlideIndex] = useState(0);

    useEffect(() => {
        // Force carousel to recalc width after mount
        setTimeout(() => {
            window.dispatchEvent(new Event('resize'));
        }, 0);
    }, []);

    //Increment/decrement functions for updating individual card counts
    function increment(cardId) {
        setCounts(prev => ({
            ...prev,
            [cardId]: (prev[cardId] || 0) + 1
        }));
    }

    function decrement(cardId) {
        setCounts(prev => ({
            ...prev,
            [cardId]: Math.max((prev[cardId] || 0) - 1, 0)
        }));
    }


    return (
        <div>
            <ReactSimplyCarousel
                activeSlideIndex={activeSlideIndex}
                onRequestChange={setActiveSlideIndex}
                itemsToShow={numItemsShow}
                itemsToScroll={numItemsScroll}
                hideNavIfAllVisible={false}
                forwardBtnProps={{ style: { backgroundColor: '#00B3B8', margin: '15px' }, children: <span>{'→'}</span> }}
                backwardBtnProps={{ style: { backgroundColor: '#00B3B8', margin: '15px' }, children: <span>{'←'}</span> }}
                speed={200}
                easing="linear"
            >
                {cards.map((card, index) => (
                    <div key={index} style={{ padding: '5px' }}>
                        <div className={container}>
                            <img src={card.image} className={cardClass} />
                        </div>
                        <h5>{card.name}</h5>
                        {selectOn && (
                            <div style={{ display: 'flex', gap: '5px', justifyContent: 'center'}}>
                                <button style={{ backgroundColor: '#00B3B8' }} type="button" onClick={() => { decrement(card.cardID) }} disabled={counts[card.cardID] <= 0}>-</button>
                                <div style={{ border: "3px solid gray", borderRadius: '2px', justifyContent: 'center', padding: '10px' }}><span>{counts[card.cardID] || 0}</span></div>
                                <button style={{ backgroundColor: '#00B3B8' }} type="button" onClick={() => { increment(card.cardID) }} disabled={counts[card.cardID] >= 10}>+</button>
                            </div>
                        )}
                    </div>
                ))}
            </ReactSimplyCarousel>
        </div>
    );


}
