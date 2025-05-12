const fs = require("fs");
const csv = require("csv-parse");
const createCsvWriter = require("csv-writer").createObjectCsvWriter;

const INPUT_FILE = "/rounds_distance.csv";

// Define output file path
const OUTPUT_FILE = INPUT_FILE.replace(".csv", "_with_speed.csv");

// Create CSV writer with headers
const csvWriter = createCsvWriter({
  path: OUTPUT_FILE,
  header: [
    { id: "id", title: "id" },
    { id: "remainingTime", title: "remainingTime" },
    { id: "dTravelled", title: "dTravelled" },
    { id: "timeTravelled", title: "timeTravelled" },
    { id: "averageSpeed", title: "averageSpeed" },
    { id: "participantId", title: "participantId" },
    { id: "phase", title: "phase" },
    { id: "pickedUpCoin", title: "pickedUpCoin" },
    { id: "finished", title: "finished" },
    { id: "totalRounds", title: "totalRounds" },
    { id: "day", title: "day" },
  ],
});

// Read and process the CSV
fs.createReadStream(INPUT_FILE)
  .pipe(
    csv.parse({
      columns: true,
      delimiter: ";", // Specify semicolon as delimiter
      trim: true, // Trim whitespace from fields
    })
  )
  .on("data", (row) => {
    // Calculate time travelled (45 - remainingTime)
    const timeTravelled = 45 - parseFloat(row.remainingTime);

    // Calculate average speed (distance / timeTravelled)
    const averageSpeed = parseFloat(row.dTravelled) / timeTravelled;

    // Add new calculations to the row
    row.timeTravelled = timeTravelled;
    row.averageSpeed = averageSpeed;

    records.push(row);
  })
  .on("end", () => {
    // Write the processed data to new CSV
    csvWriter
      .writeRecords(records)
      .then(() => console.log("CSV processing completed"));
  });

// Array to store processed records
const records = [];
