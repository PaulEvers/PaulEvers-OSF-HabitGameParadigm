const fs = require("fs");
const csv = require("csv-parse");
const createCsvWriter = require("csv-writer").createObjectCsvWriter;

const INPUT_FILE =
  // "/test.csv";
  "/rounds_roundLogs_merge_new.csv";
const OUTPUT_FILE = `rounds_consecutive_longest.csv`;

// Function to find largest consecutive increasing sequence length and start time
function findLargestConsecutiveIncreasing(logs) {
  let maxCount = 0;
  let currentCount = 0;
  let startIndex = 0;
  let maxStartIndex = 0;

  for (let i = 0; i < logs.length - 1; i++) {
    if (logs[i + 1].d > logs[i].d) {
      if (currentCount === 0) {
        startIndex = i;
      }
      currentCount++;
      if (currentCount > maxCount) {
        maxCount = currentCount;
        maxStartIndex = startIndex;
      }
    } else {
      currentCount = 0;
    }
  }

  return {
    length: maxCount + 1, // Add 1 to include the first number in the sequence
    startTime: logs[maxStartIndex]?.t || 0,
  };
}

// Process the CSV file
const processFile = async () => {
  const records = {};

  // Read and parse CSV
  const parser = fs
    .createReadStream(INPUT_FILE)
    .pipe(csv.parse({ columns: true, skip_empty_lines: true }));

  // Group records by roundId
  for await (const record of parser) {
    const roundId = record.roundId;
    if (!records[roundId]) {
      records[roundId] = {
        coinSpawnTime: parseFloat(record.coinSpawnTime),
        participantId: record.participantId,
        phase: record.phase,
        day: record.day,
        logs: [],
      };
    }
    records[roundId].logs.push({
      t: parseFloat(record.t),
      d: parseFloat(record.d),
    });
  }

  // Process each round
  const results = [];
  for (const [roundId, data] of Object.entries(records)) {
    // Filter logs after coinSpawnTime
    const relevantLogs = data.logs.filter((log) => log.t >= data.coinSpawnTime);

    // Find largest consecutive increasing sequence
    const { length: largestConsecutive, startTime: tStartLargestConsecutive } =
      findLargestConsecutiveIncreasing(relevantLogs);

    results.push({
      roundId: roundId,
      largestConsecutive: largestConsecutive,
      tStartLargestConsecutive: tStartLargestConsecutive,
      participantId: data.participantId,
      phase: data.phase,
      day: data.day,
    });
  }

  // Write results to CSV
  const csvWriter = createCsvWriter({
    path: OUTPUT_FILE,
    header: [
      { id: "roundId", title: "roundId" },
      { id: "largestConsecutive", title: "largestConsecutive" },
      { id: "tStartLargestConsecutive", title: "tStartLargestConsecutive" },
      { id: "participantId", title: "participantId" },
      { id: "phase", title: "phase" },
      { id: "day", title: "day" },
    ],
  });

  await csvWriter.writeRecords(results);
  console.log("Processing complete");
};

processFile().catch(console.error);
