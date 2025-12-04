Home page-> full list, add to user list 
| 
v 
plant of the day with weather List has plant pic, name, when opened : name, planting instructions, pics, uses, allergies


Plants Table:

CREATE TABLE `plants` (
  `PlantID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Description` text,
  `TimeToPlant` varchar(100) DEFAULT NULL,
  `AmountOfWater` varchar(200) DEFAULT NULL,
  `AmountOfSunlight` varchar(200) DEFAULT NULL,
  `TypeOfDirt` varchar(200) DEFAULT NULL,
  `TypeOfFood` varchar(200) DEFAULT NULL,
  `AnimalsAttractedRepelled` text,
  `Allergies` text,
  `Toxic` varchar(100) DEFAULT NULL,
  `UsesOfPlant` text,
  PRIMARY KEY (`PlantID`)
)

MyPlant Table:

CREATE TABLE my_plants (
    MyPlantID INT AUTO_INCREMENT PRIMARY KEY,
    PlantID   INT NOT NULL,
    CustomName VARCHAR(255),
    Instructions TEXT,
    Notes TEXT,
    LastUpdated DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_myplants_plant
        FOREIGN KEY (PlantID) REFERENCES plants (PlantID)
);
