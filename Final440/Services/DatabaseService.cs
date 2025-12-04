using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;
using Microsoft.Maui.Devices;
using Final440.Models;

namespace Final440.Services
{
    public static class DatabaseService
    {
        private static string ConnectionString =>
            DeviceInfo.Platform == DevicePlatform.Android
                ? "Server=10.0.2.2;Port=3306;Database=final440;User Id=root;Password=DOoda211!;"
                : "Server=localhost;Port=3306;Database=final440;User Id=root;Password=DOoda211!;";

        public static async Task<List<Plant>> GetAllPlantsAsync()
        {
            var plants = new List<Plant>();

            await using var conn = new MySqlConnection(ConnectionString);
            await conn.OpenAsync();

            var sql = @"SELECT PlantID, Name, Description, TimeToPlant, AmountOfWater,
                               AmountOfSunlight, TypeOfDirt, TypeOfFood,
                               AnimalsAttractedRepelled, Allergies, Toxic, UsesOfPlant
                        FROM plants
                        ORDER BY Name;";

            await using var cmd = new MySqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                plants.Add(new Plant
                {
                    PlantID = reader.GetInt32("PlantID"),
                    Name = reader.GetString("Name"),
                    Description = reader["Description"] as string,
                    TimeToPlant = reader["TimeToPlant"] as string,
                    AmountOfWater = reader["AmountOfWater"] as string,
                    AmountOfSunlight = reader["AmountOfSunlight"] as string,
                    TypeOfDirt = reader["TypeOfDirt"] as string,
                    TypeOfFood = reader["TypeOfFood"] as string,
                    AnimalsAttractedRepelled = reader["AnimalsAttractedRepelled"] as string,
                    Allergies = reader["Allergies"] as string,
                    Toxic = reader["Toxic"] as string,
                    UsesOfPlant = reader["UsesOfPlant"] as string
                });
            }

            return plants;
        }

        public static async Task AddPlantAsync(Plant plant)
        {
            await using var conn = new MySqlConnection(ConnectionString);
            await conn.OpenAsync();

            var sql = @"INSERT INTO plants
                        (Name, Description, TimeToPlant, AmountOfWater,
                         AmountOfSunlight, TypeOfDirt, TypeOfFood,
                         AnimalsAttractedRepelled, Allergies, Toxic, UsesOfPlant)
                        VALUES
                        (@Name, @Description, @TimeToPlant, @AmountOfWater,
                         @AmountOfSunlight, @TypeOfDirt, @TypeOfFood,
                         @AnimalsAttractedRepelled, @Allergies, @Toxic, @UsesOfPlant);";

            await using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Name", plant.Name);
            cmd.Parameters.AddWithValue("@Description", plant.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@TimeToPlant", plant.TimeToPlant ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AmountOfWater", plant.AmountOfWater ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AmountOfSunlight", plant.AmountOfSunlight ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@TypeOfDirt", plant.TypeOfDirt ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@TypeOfFood", plant.TypeOfFood ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AnimalsAttractedRepelled", plant.AnimalsAttractedRepelled ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Allergies", plant.Allergies ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Toxic", plant.Toxic ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@UsesOfPlant", plant.UsesOfPlant ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public static Plant? PickPlantOfTheDay(List<Plant> allPlants, DateTime now)
        {
            if (allPlants == null || allPlants.Count == 0)
                return null;

            string season = GetSeason(now.Month);

            var seasonal = allPlants
                .Where(p => (p.TimeToPlant ?? string.Empty)
                    .ToLower()
                    .Contains(season))
                .ToList();

            var source = seasonal.Count > 0 ? seasonal : allPlants;

            var index = (now.DayOfYear - 1) % source.Count;
            return source[index];
        }

        private static string GetSeason(int month)
        {
            return month switch
            {
                3 => "early spring",
                4 => "spring",
                5 => "late spring",
                6 or 7 or 8 => "summer",
                9 or 10 or 11 => "fall",
                _ => "winter"
            };
        }

        public static async Task<List<MyPlant>> GetMyPlantsAsync()
        {
            var myPlants = new List<MyPlant>();

            await using var conn = new MySqlConnection(ConnectionString);
            await conn.OpenAsync();

            var sql = @"
                SELECT mp.MyPlantID, mp.PlantID, mp.CustomName,
                       mp.Instructions, mp.Notes,
                       p.Name AS PlantName
                FROM my_plants mp
                JOIN plants p ON mp.PlantID = p.PlantID
                ORDER BY mp.MyPlantID;";

            await using var cmd = new MySqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                myPlants.Add(new MyPlant
                {
                    MyPlantID = reader.GetInt32("MyPlantID"),
                    PlantID = reader.GetInt32("PlantID"),
                    CustomName = reader["CustomName"] as string,
                    Instructions = reader["Instructions"] as string,
                    Notes = reader["Notes"] as string,
                    PlantName = reader.GetString("PlantName")
                });
            }

            return myPlants;
        }

        public static async Task AddMyPlantAsync(MyPlant myPlant)
        {
            await using var conn = new MySqlConnection(ConnectionString);
            await conn.OpenAsync();

            var sql = @"
                INSERT INTO my_plants (PlantID, CustomName, Instructions, Notes)
                VALUES (@PlantID, @CustomName, @Instructions, @Notes);";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@PlantID", myPlant.PlantID);
            cmd.Parameters.AddWithValue("@CustomName", (object?)myPlant.CustomName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Instructions", (object?)myPlant.Instructions ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Notes", (object?)myPlant.Notes ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public static async Task UpdateMyPlantAsync(MyPlant myPlant)
        {
            await using var conn = new MySqlConnection(ConnectionString);
            await conn.OpenAsync();

            var sql = @"
                UPDATE my_plants
                SET CustomName = @CustomName,
                    Instructions = @Instructions,
                    Notes = @Notes,
                    LastUpdated = CURRENT_TIMESTAMP
                WHERE MyPlantID = @MyPlantID;";

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@MyPlantID", myPlant.MyPlantID);
            cmd.Parameters.AddWithValue("@CustomName", (object?)myPlant.CustomName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Instructions", (object?)myPlant.Instructions ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Notes", (object?)myPlant.Notes ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
