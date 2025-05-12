CREATE TABLE `participants` (
  `id` integer AUTO_INCREMENT PRIMARY KEY,
  `email` varchar(255),
  `totalScore` integer,
  `characterSelect` integer,
  `trainingSeeds` varchar(500),
  `testSeeds` varchar(500)
);

CREATE TABLE `rounds` (
  `id` integer AUTO_INCREMENT PRIMARY KEY,
  `participantId` integer,
  `seed` integer,
  `round` integer,
  `didCoinSpawn` bool,
  `pickedUpCoin` bool,
  `finished` bool,
  `phase` varchar(9),
  `date` timestamp,
  `totalDistance` decimal(7,2),
  `distanceCoinSpawn` decimal(7,2),
  `remainingTime` decimal(7,2),
  `totalRoundsFinished` integer,
  `day` integer,
  `coinSpawnTime` decimal(7,2),
  `coinPickupTime` decimal(7,2),
  FOREIGN KEY (`participantId`) REFERENCES `participants` (`id`)
);

CREATE TABLE `roundLogs` (
  `roundId` integer,                    
  `t` decimal(7,2),
  `d` decimal(7,2),
  FOREIGN KEY (`roundId`) REFERENCES `rounds` (`id`)
);

CREATE TABLE `habitSurvey` (
  `id` integer AUTO_INCREMENT PRIMARY KEY,
  `participantId` integer,
  `day` integer,
  `srbai1` integer,
  `srbai2` integer,
  `srbai3` integer,
  `srbai4` integer,
  FOREIGN KEY (`participantId`) REFERENCES `participants` (`id`)
);

CREATE TABLE `pxiSurvey` (
  `id` integer AUTO_INCREMENT PRIMARY KEY,
  `participantId` integer,
  `day` integer,
  `aa` integer,
  `ch` integer,
  `ec` integer,
  `gr` integer,
  `pf` integer,
  `aut` integer,
  `cur` integer,
  `imm` integer,
  `mas` integer,
  `mea` integer,
  `enj` integer,
  FOREIGN KEY (`participantId`) REFERENCES `participants` (`id`)
);
