const gameArea =
    document.getElementById('gameArea');

const startButton =
    document.getElementById('startButton');

const resultElement =
    document.getElementById('result');

const playerNameInput =
    document.getElementById('playerName');

const scoreList =
    document.getElementById('scoreList');


let waitingForGreen = false;
let canClick = false;

let startTime;

let timeoutId;


/*
    Starter et nytt spill
*/

startButton.addEventListener('click', startGame);


function startGame() {

    const playerName =
        playerNameInput.value.trim();

    if (playerName === '') {

        alert('Skriv inn navnet ditt først.');

        return;
    }


    resultElement.textContent = '';

    gameArea.textContent =
        'Vent på grønt...';

    gameArea.className =
        'waiting';


    waitingForGreen = true;

    canClick = false;


    const delay =
        1500 + Math.random() * 3000;


    timeoutId =
        setTimeout(() => {

            waitingForGreen = false;

            canClick = true;

            startTime =
                performance.now();

            gameArea.textContent =
                'KLIKK!';

            gameArea.className =
                'ready';

        }, delay);
}


/*
    Spilleren klikker på spilleområdet
*/

gameArea.addEventListener('click', async () => {

    if (waitingForGreen) {

        clearTimeout(timeoutId);

        waitingForGreen = false;

        gameArea.textContent =
            'For tidlig! Trykk Start igjen.';

        gameArea.className =
            'tooEarly';

        return;
    }


    if (!canClick) {
        return;
    }


    const endTime =
        performance.now();


    const milliseconds =
        Math.round(
            endTime - startTime
        );


    canClick = false;


    gameArea.textContent =
        `${milliseconds} ms`;

    gameArea.className = '';


    resultElement.textContent =
        `Din reaksjonstid: ${milliseconds} ms`;


    const playerName =
        playerNameInput.value.trim();


    await saveScore(
        playerName,
        milliseconds
    );


    await loadScores();
});


/*
    Sender et resultat til API-et
*/

async function saveScore(
    playerName,
    milliseconds
) {

    const response =
        await fetch('/scores', {

            method: 'POST',

            headers: {
                'Content-Type':
                    'application/json'
            },

            body: JSON.stringify({

                playerName:
                    playerName,

                milliseconds:
                    milliseconds

            })

        });


    if (!response.ok) {

        console.error(
            'Kunne ikke lagre resultatet.'
        );
    }
}


/*
    Henter leaderboardet
*/

async function loadScores() {

    const response =
        await fetch('/scores');


    if (!response.ok) {

        console.error(
            'Kunne ikke hente leaderboard.'
        );

        return;
    }


    const scores =
        await response.json();


    renderScores(scores);
}


/*
    Viser leaderboardet
*/

function renderScores(scores) {

    scoreList.innerHTML = '';


    for (const score of scores) {

        const li =
            document.createElement('li');


        li.textContent =
            `${score.playerName}: ${score.milliseconds} ms`;


        scoreList.appendChild(li);
    }
}


/*
    Hent leaderboardet når siden åpnes
*/

loadScores();