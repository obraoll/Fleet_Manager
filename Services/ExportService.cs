using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FleetManager.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace FleetManager.Services
{
    /// <summary>
    /// Service d'export de données
    /// </summary>
    public class ExportService
    {
        /// <summary>
        /// Exporte les véhicules en CSV
        /// </summary>
        public async Task<(bool Success, string Message)> ExportVehiclesToCsvAsync(List<Vehicle> vehicles, string filePath)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                };

                await using var writer = new StreamWriter(filePath);
                await using var csv = new CsvWriter(writer, config);

                // Écrire les en-têtes
                csv.WriteField("Immatriculation");
                csv.WriteField("Marque");
                csv.WriteField("Modèle");
                csv.WriteField("Année");
                csv.WriteField("Type");
                csv.WriteField("Carburant");
                csv.WriteField("Kilométrage");
                csv.WriteField("Consommation Moyenne");
                csv.WriteField("Statut");
                await csv.NextRecordAsync();

                // Écrire les données
                foreach (var vehicle in vehicles)
                {
                    csv.WriteField(vehicle.RegistrationNumber);
                    csv.WriteField(vehicle.Brand);
                    csv.WriteField(vehicle.Model);
                    csv.WriteField(vehicle.Year);
                    csv.WriteField(vehicle.VehicleType.ToString());
                    csv.WriteField(vehicle.FuelType.ToString());
                    csv.WriteField(vehicle.CurrentMileage);
                    csv.WriteField(vehicle.AverageFuelConsumption);
                    csv.WriteField(vehicle.Status.ToString());
                    await csv.NextRecordAsync();
                }

                return (true, "Export CSV réussi.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de l'export CSV: {ex.Message}");
            }
        }

        /// <summary>
        /// Exporte les statistiques des véhicules en CSV
        /// </summary>
        public async Task<(bool Success, string Message)> ExportStatisticsToCsvAsync(List<VehicleStatistics> statistics, string filePath)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                };

                await using var writer = new StreamWriter(filePath);
                await using var csv = new CsvWriter(writer, config);

                // Écrire les en-têtes
                csv.WriteField("Véhicule");
                csv.WriteField("Immatriculation");
                csv.WriteField("Kilométrage");
                csv.WriteField("Nombre de Pleins");
                csv.WriteField("Litres Total");
                csv.WriteField("Coût Carburant");
                csv.WriteField("Consommation Moyenne (L/100km)");
                csv.WriteField("Prix Moyen par Litre");
                csv.WriteField("Nombre Maintenances");
                csv.WriteField("Coût Maintenance");
                csv.WriteField("Coût Total");
                csv.WriteField("Coût par Kilomètre");
                csv.WriteField("Efficacité (km/L)");
                csv.WriteField("Dernière Maintenance");
                csv.WriteField("Prochaine Maintenance");
                await csv.NextRecordAsync();

                // Écrire les données
                foreach (var stat in statistics)
                {
                    csv.WriteField(stat.VehicleName);
                    csv.WriteField(stat.RegistrationNumber);
                    csv.WriteField(stat.CurrentMileage.ToString("F0"));
                    csv.WriteField(stat.TotalRefuels);
                    csv.WriteField(stat.TotalLiters.ToString("F2"));
                    csv.WriteField(stat.TotalFuelCost.ToString("F2"));
                    csv.WriteField(stat.AverageConsumption.ToString("F2"));
                    csv.WriteField(stat.AveragePricePerLiter.ToString("F2"));
                    csv.WriteField(stat.TotalMaintenances);
                    csv.WriteField(stat.TotalMaintenanceCost.ToString("F2"));
                    csv.WriteField(stat.TotalCost.ToString("F2"));
                    csv.WriteField(stat.CostPerKilometer.ToString("F4"));
                    csv.WriteField(stat.FuelEfficiency.ToString("F2"));
                    csv.WriteField(stat.LastMaintenanceDate?.ToString("dd/MM/yyyy") ?? "");
                    csv.WriteField(stat.NextMaintenanceDate?.ToString("dd/MM/yyyy") ?? "");
                    await csv.NextRecordAsync();
                }

                return (true, "Export des statistiques réussi.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de l'export des statistiques: {ex.Message}");
            }
        }

        /// <summary>
        /// Exporte les statistiques mensuelles en CSV
        /// </summary>
        public async Task<(bool Success, string Message)> ExportMonthllyStatisticsToCsvAsync(List<MonthlyStatistics> monthlyStats, string filePath)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                };

                await using var writer = new StreamWriter(filePath);
                await using var csv = new CsvWriter(writer, config);

                // Écrire les en-têtes
                csv.WriteField("Année");
                csv.WriteField("Mois");
                csv.WriteField("Nom du Mois");
                csv.WriteField("Coût Carburant");
                csv.WriteField("Coût Maintenance");
                csv.WriteField("Coût Total");
                csv.WriteField("Litres Total");
                csv.WriteField("Consommation Moyenne");
                csv.WriteField("Kilométrage Total");
                csv.WriteField("Nombre de Pleins");
                csv.WriteField("Nombre de Maintenances");
                await csv.NextRecordAsync();

                // Écrire les données
                foreach (var stat in monthlyStats)
                {
                    csv.WriteField(stat.Year);
                    csv.WriteField(stat.Month);
                    csv.WriteField(stat.MonthName);
                    csv.WriteField(stat.FuelCost.ToString("F2"));
                    csv.WriteField(stat.MaintenanceCost.ToString("F2"));
                    csv.WriteField(stat.TotalCost.ToString("F2"));
                    csv.WriteField(stat.TotalLiters.ToString("F2"));
                    csv.WriteField(stat.AverageConsumption.ToString("F2"));
                    csv.WriteField(stat.TotalMileage.ToString("F0"));
                    csv.WriteField(stat.RefuelCount);
                    csv.WriteField(stat.MaintenanceCount);
                    await csv.NextRecordAsync();
                }

                return (true, "Export des statistiques mensuelles réussi.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de l'export des statistiques mensuelles: {ex.Message}");
            }
        }

        /// <summary>
        /// Génère un rapport PDF simple (iText 9 compatible)
        /// </summary>
        public (bool Success, string Message) GeneratePdfReport(string title, string content, string filePath)
        {
            try
            {
                using var writer = new PdfWriter(filePath);
                using var pdf = new PdfDocument(writer);
                using var document = new Document(pdf);

                // Titre
                document.Add(new Paragraph(title)
                    .SetFontSize(18)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20));

                // Contenu
                document.Add(new Paragraph(content)
                    .SetFontSize(12));

                return (true, "Rapport PDF généré avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de la génération du PDF: {ex.Message}");
            }
        }

        /// <summary>
        /// Génère un rapport PDF détaillé avec statistiques (iText 9 - version simplifiée)
        /// </summary>
        public (bool Success, string Message) GenerateAdvancedPdfReport(string title, FleetStatistics fleetStats, List<VehicleStatistics> vehicleStats, string filePath)
        {
            try
            {
                using var writer = new PdfWriter(filePath);
                using var pdf = new PdfDocument(writer);
                using var document = new Document(pdf);

                // Titre
                document.Add(new Paragraph(title)
                    .SetFontSize(20)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(15));

                // Date
                document.Add(new Paragraph($"Généré le: {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetMarginBottom(20));

                // Section statistiques globales
                document.Add(new Paragraph("STATISTIQUES GLOBALES")
                    .SetFontSize(16)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetMarginTop(20)
                    .SetMarginBottom(10));

                document.Add(new Paragraph()
                    .Add($"Nombre total de véhicules: {fleetStats.TotalVehicles}\n")
                    .Add($"Véhicules actifs: {fleetStats.ActiveVehicles}\n")
                    .Add($"Véhicules en maintenance: {fleetStats.VehiclesInMaintenance}\n")
                    .Add($"Coût total carburant: {fleetStats.TotalFuelCost:C}\n")
                    .Add($"Coût total maintenance: {fleetStats.TotalMaintenanceCost:C}\n")
                    .Add($"Consommation moyenne flotte: {fleetStats.AverageFleetConsumption:F2} L/100km\n")
                    .Add($"Kilométrage total: {fleetStats.TotalMileage:N0} km\n")
                    .SetFontSize(12)
                    .SetMarginBottom(20));

                // Top véhicules
                document.Add(new Paragraph("TOP 10 VEHICULES PAR COUT")
                    .SetFontSize(16)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetMarginTop(20)
                    .SetMarginBottom(10));

                // Tableau simple
                var table = new Table(5).UseAllAvailableWidth();
                table.AddHeaderCell("Véhicule");
                table.AddHeaderCell("Immatriculation");
                table.AddHeaderCell("Consommation");
                table.AddHeaderCell("Coût Total");
                table.AddHeaderCell("€/km");

                foreach (var vehicle in vehicleStats.OrderByDescending(v => v.TotalCost).Take(10))
                {
                    table.AddCell(vehicle.VehicleName);
                    table.AddCell(vehicle.RegistrationNumber);
                    table.AddCell($"{vehicle.AverageConsumption:F2} L/100km");
                    table.AddCell($"{vehicle.TotalCost:C}");
                    table.AddCell($"{vehicle.CostPerKilometer:C}");
                }

                document.Add(table);

                // Footer
                document.Add(new Paragraph($"\n\nRapport généré par Fleet Manager - {DateTime.Now:dd/MM/yyyy}")
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginTop(30));

                return (true, "Rapport PDF avancé généré avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de la génération du PDF avancé: {ex.Message}");
            }
        }

        /// <summary>
        /// Génère un rapport PDF simple (legacy signature kept for compatibility)
        /// </summary>
        private (bool Success, string Message) GeneratePdfReportLegacy(string title, string content, string filePath)
        {
            return GeneratePdfReport(title, content, filePath);
        }

        /// <summary>
        /// Exporte les comparaisons de performance en CSV
        /// </summary>
        public async Task<(bool Success, string Message)> ExportPerformanceComparisonsToCsvAsync(List<PerformanceComparison> comparisons, string filePath)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                };

                await using var writer = new StreamWriter(filePath);
                await using var csv = new CsvWriter(writer, config);

                // Écrire les en-têtes
                csv.WriteField("Véhicule");
                csv.WriteField("Consommation vs Flotte (%)");
                csv.WriteField("Coût vs Flotte (%)");
                csv.WriteField("Note Efficacité");
                csv.WriteField("Grade Performance");
                csv.WriteField("Recommandations");
                await csv.NextRecordAsync();

                // Écrire les données
                foreach (var comparison in comparisons)
                {
                    csv.WriteField(comparison.VehicleRegistration);
                    csv.WriteField(comparison.ConsumptionVsFleet.ToString("F1"));
                    csv.WriteField(comparison.CostVsFleet.ToString("F1"));
                    csv.WriteField(comparison.EfficiencyRating.ToString("F2"));
                    csv.WriteField(comparison.PerformanceGrade);
                    csv.WriteField(string.Join("; ", comparison.Recommendations));
                    await csv.NextRecordAsync();
                }

                return (true, "Export des comparaisons de performance réussi.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de l'export des comparaisons: {ex.Message}");
            }
        }
    }
}
