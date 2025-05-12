const fs = require("fs");
const csv = require("csv-parse");
const createCsvWriter = require("csv-writer").createObjectCsvWriter;

// Configuration
const THRESHOLD = 8; // Number of consecutive decreasing distances required
const INPUT_FILE =
  // "/test.csv";
  "/rounds_roundLogs_merge_new.csv";
const OUTPUT_FILE = `rounds_t${THRESHOLD}.csv`;

async function processData() {
  // Read and parse CSV
  const records = await new Promise((resolve, reject) => {
    const results = [];
    fs.createReadStream(INPUT_FILE)
      .pipe(csv.parse({ columns: true }))
      .on("data", (data) => {
        results.push({
          ...data,
          // Convert string values to numbers
          t: parseFloat(data.t),
          d: parseFloat(data.d),
          coinSpawnTime: parseFloat(data.coinSpawnTime),
          roundId: parseInt(data.roundId),
        });
      })
      .on("end", () => resolve(results))
      .on("error", reject);
  });

  // Group records by roundId
  const roundGroups = {};
  records.forEach((record) => {
    if (!roundGroups[record.roundId]) {
      roundGroups[record.roundId] = [];
    }
    roundGroups[record.roundId].push(record);
  });

  // Process each round
  const processedRecords = [];
  for (const roundId in roundGroups) {
    const roundData = roundGroups[roundId];
    // Sort by time (although data should already be chronological)
    roundData.sort((a, b) => a.t - b.t);

    // Process for both Test and Training phases
    if (roundData[0].phase === "Test" || roundData[0].phase === "Training") {
      // Only look at records after coinSpawnTime
      const relevantData = roundData.filter(
        (record) => record.t >= record.coinSpawnTime
      );

      let wentBack = false;
      let actionSlipStartTime = null;
      let forwardStartTime = null;

      // First check for going back (increasing distance)
      for (let i = 0; i < relevantData.length - THRESHOLD; i++) {
        let isIncreasing = true;

        // Check if next THRESHOLD records show increasing distance
        for (let j = 0; j < THRESHOLD - 1; j++) {
          if (relevantData[i + j].d >= relevantData[i + j + 1].d) {
            isIncreasing = false;
            break;
          }
        }

        if (isIncreasing) {
          wentBack = true;

          // Look backwards to find when the stable period started
          let stableStartIndex = i;
          while (
            stableStartIndex > 0 &&
            relevantData[stableStartIndex - 1].d ===
              relevantData[stableStartIndex].d
          ) {
            stableStartIndex--;
          }

          actionSlipStartTime =
            relevantData[stableStartIndex].t -
            relevantData[stableStartIndex].coinSpawnTime;
          break;
        }
      }

      // If didn't go back, check for stable period within 2 seconds after coinSpawnTime
      if (!wentBack) {
        // Get logs within 2 seconds after coinSpawnTime
        const twoSecondWindow = relevantData.filter(
          (record) => record.t <= record.coinSpawnTime + 2
        );

        // Look through the window for a sequence of stable logs
        for (let i = 0; i < twoSecondWindow.length - THRESHOLD; i++) {
          let stableCount = 1; // Start counting the current position
          const baseDistance = twoSecondWindow[i].d;

          // Count how many consecutive logs have the same distance
          while (
            i + stableCount < twoSecondWindow.length &&
            twoSecondWindow[i + stableCount].d === baseDistance
          ) {
            stableCount++;
          }

          // If we found a stable sequence of at least THRESHOLD length
          if (stableCount >= THRESHOLD) {
            // Use the last log of the stable sequence as RT
            forwardStartTime =
              twoSecondWindow[i + stableCount - 1].t -
              twoSecondWindow[i + stableCount - 1].coinSpawnTime;
            break;
          }
        }
      }

      // Add record for all Test and Training phase rounds
      processedRecords.push({
        ...roundData[0],
        wentBack: wentBack ? 1 : 0,
        hasActionSlip: roundData[0].phase === "Test" && wentBack ? 1 : 0,
        rtReturn: wentBack ? parseFloat(actionSlipStartTime.toFixed(3)) : null,
        rtForward:
          !wentBack && forwardStartTime
            ? parseFloat(forwardStartTime.toFixed(3))
            : null,
        rt: wentBack
          ? parseFloat(actionSlipStartTime.toFixed(3))
          : forwardStartTime
          ? parseFloat(forwardStartTime.toFixed(3))
          : null,
      });
    }
  }

  // Write results to new CSV
  const csvWriter = createCsvWriter({
    path: OUTPUT_FILE,
    header: [
      { id: "roundId", title: "roundId" },
      { id: "phase", title: "phase" },
      { id: "day", title: "day" },
      { id: "t", title: "t" },
      { id: "d", title: "d" },
      { id: "coinSpawnTime", title: "coinSpawnTime" },
      { id: "wentBack", title: "wentBack" },
      { id: "hasActionSlip", title: "hasActionSlip" },
      { id: "rtReturn", title: "rtReturn" },
      { id: "rtForward", title: "rtForward" },
      { id: "rt", title: "rt" },
      { id: "participantId", title: "participantId" },
      { id: "totalRounds", title: "totalRounds" },
    ],
  });

  await csvWriter.writeRecords(processedRecords);
  console.log(
    `Processing complete! Check rounds_t${THRESHOLD}.csv for results.`
  );
}

processData().catch(console.error);
