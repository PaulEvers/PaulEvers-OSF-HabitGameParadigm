const fs = require("fs");
const csv = require("csv-parser");
const { parse } = require("json2csv");

// Load the CSV file
const inputFile =
  // "/test.csv";
  "/rounds_roundLogs_merge_new.csv";
const outputFile = "output.csv";

// Parameters
const THRESHOLD = 1; // Number of consecutive timestamps for the action slip

const processData = async () => {
  const rows = [];

  // Step 1: Load CSV data
  fs.createReadStream(inputFile)
    .pipe(csv())
    .on("data", (data) => rows.push(data))
    .on("end", () => {
      const processedRoundIds = new Set();
      // Step 2: Process rows
      const processedRows = rows.map((row, index) => {
        const coinSpawnTime = parseFloat(row.coinSpawnTime);
        const currentDistance = parseFloat(row.d);
        const currentTime = parseFloat(row.t);
        const time = parseFloat(row.t);
        const roundId = row.roundId;

        let actionSlip = false;
        let rtThreshold = null;

        if (processedRoundIds.has(roundId)) {
          // processedRows.push({ ...row, actionSlip, rtThreshold });
          return;
        }

        // Only process if coinSpawnTime is less than or equal to current time
        if (coinSpawnTime <= time) {
          let increasingCount = 0;
          let decreasingCount = 0;
          let reactionTime = null;
          let firstChangeTime = null;

          for (let i = index + 1; i < rows.length; i++) {
            const currentRow = rows[i - 1];
            const nextRow = rows[i];

            const nextRoundId = nextRow.roundId;
            if (nextRoundId !== roundId) break;

            const nextTime = parseFloat(nextRow.t);
            const nextDistance = parseFloat(nextRow.d);

            // Check if distance increases or decreases
            if (nextDistance >= currentDistance) {
              increasingCount++;
              decreasingCount = 0;
              if (increasingCount === 1) {
                if (currentRow.t == coinSpawnTime) {
                  firstChangeTime = currentTime;
                } else {
                  firstChangeTime = currentRow.t;
                }
              }
            }
            // else if (nextDistance <= currentDistance) {
            //   decreasingCount++;
            //   increasingCount = 0; // Reset increasing count
            //   if (decreasingCount === 1) {
            //     if (currentRow.t == coinSpawnTime) {
            //       firstChangeTime = currentTime;
            //     } else {
            //       firstChangeTime = nextRow.t;
            //     }
            //   }
            // }
            else {
              increasingCount = 0;
              decreasingCount = 0;
            }

            // Check if the threshold is reached
            if (increasingCount >= THRESHOLD || decreasingCount >= THRESHOLD) {
              reactionTime = firstChangeTime - coinSpawnTime;
              rtThreshold = reactionTime; // Save the reaction time
              if (increasingCount >= THRESHOLD && reactionTime > 0) {
                actionSlip = true;
              }

              processedRoundIds.add(roundId);
              break;
            }
          }
        }

        // Add new columns to the row; only keep rtThreshold if threshold is reached
        return {
          ...row,
          actionSlip,
          rtThreshold,
        };
      });

      // Step 3: Write updated data to a new CSV file
      const fields = Object.keys(processedRows[0]);
      const opts = { fields };
      const csvOutput = parse(processedRows, opts);

      fs.writeFileSync(outputFile, csvOutput);
      console.log(`Processed data saved to ${outputFile}`);
    });
};

processData();
