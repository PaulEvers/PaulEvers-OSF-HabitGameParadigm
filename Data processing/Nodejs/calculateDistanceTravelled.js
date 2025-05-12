const fs = require("fs");
const csv = require("csv-parse");
const createCsvWriter = require("csv-writer").createObjectCsvWriter;

const INPUT_FILE = "/rounds_roundLogs_merge_new.csv";

const csvWriter = createCsvWriter({
  path: "total_distance_per_round.csv",
  header: [
    { id: "roundId", title: "roundId" },
    { id: "dTravelled", title: "dTravelled" },
  ],
});

// Store the last distance and total travelled distance per round
const roundData = new Map();

fs.createReadStream(INPUT_FILE)
  .pipe(csv.parse({ columns: true }))
  .on("data", (row) => {
    const roundId = row.roundId;
    const currentDistance = parseFloat(row.d);

    if (!roundData.has(roundId)) {
      roundData.set(roundId, {
        lastDistance: currentDistance,
        totalTravelled: 0,
      });
    } else {
      const data = roundData.get(roundId);
      // Add absolute difference to total
      const distanceDiff = Math.abs(currentDistance - data.lastDistance);
      data.totalTravelled += distanceDiff;
      data.lastDistance = currentDistance;
      roundData.set(roundId, data);
    }
  })
  .on("end", async () => {
    const records = Array.from(roundData.entries()).map(([roundId, data]) => ({
      roundId,
      dTravelled: parseFloat(data.totalTravelled.toFixed(3)),
    }));

    try {
      await csvWriter.writeRecords(records);
      console.log("CSV file has been written successfully");
    } catch (err) {
      console.error("Error writing CSV:", err);
    }
  });
