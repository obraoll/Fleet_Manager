using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManager.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetManager.Services
{
    /// <summary>
    /// Service de gestion des enregistrements de carburant
    /// </summary>
    public class FuelService
    {
        private readonly FleetDbContext _context;

        public FuelService(FleetDbContext context)
        {
            _context = context;
        }

        public async Task<List<FuelRecord>> GetAllFuelRecordsAsync()
        {
            return await _context.FuelRecords
                .Include(f => f.Vehicle)
                .OrderByDescending(f => f.RefuelDate)
                .ToListAsync();
        }

        public async Task<List<FuelRecord>> GetFuelRecordsByVehicleAsync(int vehicleId)
        {
            return await _context.FuelRecords
                .Where(f => f.VehicleId == vehicleId)
                .OrderByDescending(f => f.RefuelDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalFuelCostAsync()
        {
            try
            {
                return await _context.FuelRecords.SumAsync(f => f.TotalCost);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<(bool Success, string Message)> AddFuelRecordAsync(FuelRecord fuelRecord, int userId)
        {
            try
            {
                fuelRecord.CreatedBy = userId;
                fuelRecord.CreatedAt = DateTime.Now;
                fuelRecord.TotalCost = fuelRecord.LitersRefueled * fuelRecord.PricePerLiter;

                // Calculer la consommation
                await CalculateConsumptionAsync(fuelRecord);

                _context.FuelRecords.Add(fuelRecord);
                await _context.SaveChangesAsync();

                // Mettre à jour le kilométrage du véhicule
                var vehicle = await _context.Vehicles.FindAsync(fuelRecord.VehicleId);
                if (vehicle != null && fuelRecord.Mileage > vehicle.CurrentMileage)
                {
                    vehicle.CurrentMileage = fuelRecord.Mileage;
                    await _context.SaveChangesAsync();
                }

                return (true, "Plein de carburant enregistré avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur: {ex.Message}");
            }
        }

        private async Task CalculateConsumptionAsync(FuelRecord fuelRecord)
        {
            var lastRecord = await _context.FuelRecords
                .Where(f => f.VehicleId == fuelRecord.VehicleId && f.Mileage < fuelRecord.Mileage)
                .OrderByDescending(f => f.Mileage)
                .FirstOrDefaultAsync();

            if (lastRecord != null && fuelRecord.IsFullTank)
            {
                var distance = fuelRecord.Mileage - lastRecord.Mileage;
                if (distance > 0)
                {
                    fuelRecord.DistanceSinceLastRefuel = distance;
                    fuelRecord.CalculatedConsumption = (fuelRecord.LitersRefueled / distance) * 100;
                }
            }
        }

        public async Task<(bool Success, string Message)> DeleteFuelRecordAsync(int fuelRecordId)
        {
            try
            {
                var record = await _context.FuelRecords.FindAsync(fuelRecordId);
                if (record == null)
                    return (false, "Enregistrement introuvable.");

                _context.FuelRecords.Remove(record);
                await _context.SaveChangesAsync();
                return (true, "Enregistrement supprimé.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur: {ex.Message}");
            }
        }
    }
}
