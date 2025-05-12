const fs = require("fs");
const { parse } = require("csv-parse");
const createCsvWriter = require("csv-writer").createObjectCsvWriter;

const INPUT_FILE =
  //   "C:/Users/paulu/OneDrive - TU Eindhoven/TUe/Thesis/Data/Nodejs/test.csv";
  "C:/Users/paulu/OneDrive - TU Eindhoven/TUe/Thesis/Data/Nodejs/rounds_roundLogs_merge_new.csv";
const OUTPUT_FILE = `rounds_consecutive.csv`;

const THRESHOLD = 5;

// Configure CSV writer
const csvWriter = createCsvWriter({
  path: OUTPUT_FILE,
  header: [
    { id: "roundId", title: "roundId" },
    { id: "firstConsecutive", title: "firstConsecutive" },
    { id: "tStartConsecutive", title: "tStartConsecutive" },
  ],
});

// Process the data by roundId
function processRoundData(roundData) {
  const coinSpawnTime = parseFloat(roundData[0].coinSpawnTime);

  // Find first non-decreasing sequence after coinSpawnTime
  let startIndex = -1;
  let consecutiveCount = 0;

  for (let i = 1; i < roundData.length; i++) {
    const currentTime = parseFloat(roundData[i].t);
    if (currentTime < coinSpawnTime) continue;

    const currentD = parseFloat(roundData[i].d);
    const prevD = parseFloat(roundData[i - 1].d);

    if (currentD >= prevD) {
      if (startIndex === -1) {
        startIndex = i;
        consecutiveCount = 2;
      } else {
        consecutiveCount++;
      }
    } else {
      if (startIndex !== -1) break;
    }
  }

  return {
    roundId: roundData[0].roundId,
    firstConsecutive: consecutiveCount >= THRESHOLD ? consecutiveCount : null,
    tStartConsecutive:
      consecutiveCount >= THRESHOLD ? roundData[startIndex - 1].t : null,
  };
}

// Read and process the CSV
const results = [];
let currentRound = [];
let currentRoundId = null;

fs.createReadStream(INPUT_FILE)
  .pipe(parse({ columns: true }))
  .on("data", (row) => {
    if (currentRoundId !== row.roundId) {
      if (currentRound.length > 0) {
        results.push(processRoundData(currentRound));
      }
      currentRoundId = row.roundId;
      currentRound = [];
    }
    currentRound.push(row);
  })
  .on("end", () => {
    // Process the last round
    if (currentRound.length > 0) {
      results.push(processRoundData(currentRound));
    }

    // Write results to file
    csvWriter
      .writeRecords(results)
      .then(() => console.log("CSV file written successfully"));
  });
