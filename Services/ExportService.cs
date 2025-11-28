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
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

namespace FleetManager.Services
{
    /// <summary>
    /// Service d'export de données - Version améliorée et professionnelle
    /// </summary>
    public class ExportService
    {
        #region Export CSV Améliorés

        /// <summary>
        /// Exporte les véhicules en CSV avec tous les détails
        /// </summary>
        public async Task<(bool Success, string Message)> ExportVehiclesToCsvAsync(List<Vehicle> vehicles, string filePath)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.GetCultureInfo("fr-FR"))
                {
                    Delimiter = ";",
                    HasHeaderRecord = true
                };

                await using var writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                await using var csv = new CsvWriter(writer, config);

                // Écrire les en-têtes enrichis
                csv.WriteField("Immatriculation");
                csv.WriteField("Marque");
                csv.WriteField("Modèle");
                csv.WriteField("Année");
                csv.WriteField("Type de Véhicule");
                csv.WriteField("Type de Carburant");
                csv.WriteField("Kilométrage Actuel");
                csv.WriteField("Capacité Réservoir (L)");
                csv.WriteField("Consommation Moyenne (L/100km)");
                csv.WriteField("Statut");
                csv.WriteField("Date d'Achat");
                csv.WriteField("Prix d'Achat");
                csv.WriteField("Date Expiration Assurance");
                csv.WriteField("Date Contrôle Technique");
                csv.WriteField("Notes");
                csv.WriteField("Date de Création");
                await csv.NextRecordAsync();

                // Écrire les données
                foreach (var vehicle in vehicles)
                {
                    csv.WriteField(vehicle.RegistrationNumber);
                    csv.WriteField(vehicle.Brand);
                    csv.WriteField(vehicle.Model);
                    csv.WriteField(vehicle.Year);
                    csv.WriteField(vehicle.VehicleType);
                    csv.WriteField(vehicle.FuelType);
                    csv.WriteField(vehicle.CurrentMileage.ToString("F2"));
                    csv.WriteField(vehicle.TankCapacity.ToString("F2"));
                    csv.WriteField(vehicle.AverageFuelConsumption.ToString("F2"));
                    csv.WriteField(vehicle.Status);
                    csv.WriteField(vehicle.PurchaseDate?.ToString("dd/MM/yyyy") ?? "");
                    csv.WriteField(vehicle.PurchasePrice?.ToString("F2") ?? "");
                    csv.WriteField(vehicle.InsuranceExpiryDate?.ToString("dd/MM/yyyy") ?? "");
                    csv.WriteField(vehicle.TechnicalInspectionDate?.ToString("dd/MM/yyyy") ?? "");
                    csv.WriteField(vehicle.Notes ?? "");
                    csv.WriteField(vehicle.CreatedAt.ToString("dd/MM/yyyy HH:mm"));
                    await csv.NextRecordAsync();
                }

                return (true, $"Export CSV réussi: {vehicles.Count} véhicule(s) exporté(s).");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de l'export CSV: {ex.Message}");
            }
        }

        /// <summary>
        /// Exporte les statistiques des véhicules en CSV avec toutes les métriques
        /// </summary>
        public async Task<(bool Success, string Message)> ExportStatisticsToCsvAsync(List<VehicleStatistics> statistics, string filePath)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.GetCultureInfo("fr-FR"))
                {
                    Delimiter = ";",
                    HasHeaderRecord = true
                };

                await using var writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                await using var csv = new CsvWriter(writer, config);

                // Écrire les en-têtes complets
                csv.WriteField("Véhicule");
                csv.WriteField("Immatriculation");
                csv.WriteField("Marque");
                csv.WriteField("Modèle");
                csv.WriteField("Kilométrage Actuel");
                csv.WriteField("Nombre de Pleins");
                csv.WriteField("Litres Totaux");
                csv.WriteField("Coût Carburant Total");
                csv.WriteField("Consommation Moyenne (L/100km)");
                csv.WriteField("Prix Moyen par Litre");
                csv.WriteField("Nombre de Maintenances");
                csv.WriteField("Coût Maintenance Total");
                csv.WriteField("Coût Total d'Exploitation");
                csv.WriteField("Coût par Kilomètre");
                csv.WriteField("Efficacité Énergétique (km/L)");
                csv.WriteField("Dernière Maintenance");
                csv.WriteField("Prochaine Maintenance");
                csv.WriteField("Kilométrage Prochaine Maintenance");
                csv.WriteField("Jours Depuis Dernière Maintenance");
                csv.WriteField("Performance (A-F)");
                await csv.NextRecordAsync();

                // Écrire les données
                foreach (var stat in statistics)
                {
                    csv.WriteField(stat.VehicleName);
                    csv.WriteField(stat.RegistrationNumber);
                    csv.WriteField(stat.Model ?? "");
                    csv.WriteField(stat.VehicleType ?? "");
                    csv.WriteField(stat.CurrentMileage.ToString("F0"));
                    csv.WriteField(stat.TotalRefuels);
                    csv.WriteField(stat.TotalLiters.ToString("F2"));
                    csv.WriteField(stat.TotalFuelCost.ToString("F2"));
                    csv.WriteField(stat.AverageConsumption.ToString("F2"));
                    csv.WriteField(stat.AveragePricePerLiter.ToString("F3"));
                    csv.WriteField(stat.TotalMaintenances);
                    csv.WriteField(stat.TotalMaintenanceCost.ToString("F2"));
                    csv.WriteField(stat.TotalCost.ToString("F2"));
                    csv.WriteField(stat.CostPerKilometer.ToString("F4"));
                    csv.WriteField(stat.FuelEfficiency.ToString("F2"));
                    csv.WriteField(stat.LastMaintenanceDate?.ToString("dd/MM/yyyy") ?? "Jamais");
                    csv.WriteField(stat.NextMaintenanceDate?.ToString("dd/MM/yyyy") ?? "Non planifiée");
                    csv.WriteField(stat.NextMaintenanceMileage?.ToString("F0") ?? "");
                    csv.WriteField(stat.DaysSinceLastMaintenance.ToString());
                    csv.WriteField(CalculatePerformanceGrade(stat.AverageConsumption, stat.CostPerKilometer));
                    await csv.NextRecordAsync();
                }

                // Ajouter une ligne de totaux
                await csv.NextRecordAsync();
                csv.WriteField("TOTAUX/MOYENNES");
                csv.WriteField("");
                csv.WriteField("");
                csv.WriteField("");
                csv.WriteField(statistics.Sum(s => s.CurrentMileage).ToString("F0"));
                csv.WriteField(statistics.Sum(s => s.TotalRefuels));
                csv.WriteField(statistics.Sum(s => s.TotalLiters).ToString("F2"));
                csv.WriteField(statistics.Sum(s => s.TotalFuelCost).ToString("F2"));
                csv.WriteField(statistics.Average(s => s.AverageConsumption).ToString("F2"));
                csv.WriteField(statistics.Average(s => s.AveragePricePerLiter).ToString("F3"));
                csv.WriteField(statistics.Sum(s => s.TotalMaintenances));
                csv.WriteField(statistics.Sum(s => s.TotalMaintenanceCost).ToString("F2"));
                csv.WriteField(statistics.Sum(s => s.TotalCost).ToString("F2"));
                csv.WriteField(statistics.Average(s => s.CostPerKilometer).ToString("F4"));
                csv.WriteField(statistics.Average(s => s.FuelEfficiency).ToString("F2"));
                await csv.NextRecordAsync();

                return (true, $"Export des statistiques réussi: {statistics.Count} véhicule(s) analysé(s).");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de l'export des statistiques: {ex.Message}");
            }
        }

        /// <summary>
        /// Exporte les statistiques mensuelles en CSV enrichi
        /// </summary>
        public async Task<(bool Success, string Message)> ExportMonthlyStatisticsToCsvAsync(List<MonthlyStatistics> monthlyStats, string filePath)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.GetCultureInfo("fr-FR"))
                {
                    Delimiter = ";",
                    HasHeaderRecord = true
                };

                await using var writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                await using var csv = new CsvWriter(writer, config);

                // Écrire les en-têtes
                csv.WriteField("Année");
                csv.WriteField("Mois");
                csv.WriteField("Nom du Mois");
                csv.WriteField("Coût Carburant (€)");
                csv.WriteField("Coût Maintenance (€)");
                csv.WriteField("Coût Total (€)");
                csv.WriteField("Litres Consommés");
                csv.WriteField("Consommation Moyenne (L/100km)");
                csv.WriteField("Kilométrage Total");
                csv.WriteField("Nombre de Pleins");
                csv.WriteField("Nombre de Maintenances");
                csv.WriteField("Prix Moyen Litre (€)");
                csv.WriteField("Coût/km (€)");
                csv.WriteField("% Carburant/Total");
                csv.WriteField("% Maintenance/Total");
                await csv.NextRecordAsync();

                // Écrire les données
                foreach (var stat in monthlyStats.OrderBy(s => s.Year).ThenBy(s => s.Month))
                {
                    var totalCost = stat.FuelCost + stat.MaintenanceCost;
                    var avgPricePerLiter = stat.TotalLiters > 0 ? stat.FuelCost / stat.TotalLiters : 0;
                    var costPerKm = stat.TotalMileage > 0 ? totalCost / stat.TotalMileage : 0;
                    var fuelPercentage = totalCost > 0 ? (stat.FuelCost / totalCost * 100) : 0;
                    var maintenancePercentage = totalCost > 0 ? (stat.MaintenanceCost / totalCost * 100) : 0;

                    csv.WriteField(stat.Year);
                    csv.WriteField(stat.Month);
                    csv.WriteField(stat.MonthName);
                    csv.WriteField(stat.FuelCost.ToString("F2"));
                    csv.WriteField(stat.MaintenanceCost.ToString("F2"));
                    csv.WriteField(totalCost.ToString("F2"));
                    csv.WriteField(stat.TotalLiters.ToString("F2"));
                    csv.WriteField(stat.AverageConsumption.ToString("F2"));
                    csv.WriteField(stat.TotalMileage.ToString("F0"));
                    csv.WriteField(stat.RefuelCount);
                    csv.WriteField(stat.MaintenanceCount);
                    csv.WriteField(avgPricePerLiter.ToString("F3"));
                    csv.WriteField(costPerKm.ToString("F4"));
                    csv.WriteField(fuelPercentage.ToString("F1"));
                    csv.WriteField(maintenancePercentage.ToString("F1"));
                    await csv.NextRecordAsync();
                }

                // Totaux annuels
                await csv.NextRecordAsync();
                csv.WriteField("TOTAL ANNUEL");
                csv.WriteField("");
                csv.WriteField("");
                csv.WriteField(monthlyStats.Sum(s => s.FuelCost).ToString("F2"));
                csv.WriteField(monthlyStats.Sum(s => s.MaintenanceCost).ToString("F2"));
                csv.WriteField(monthlyStats.Sum(s => s.FuelCost + s.MaintenanceCost).ToString("F2"));
                csv.WriteField(monthlyStats.Sum(s => s.TotalLiters).ToString("F2"));
                csv.WriteField(monthlyStats.Average(s => s.AverageConsumption).ToString("F2"));
                csv.WriteField(monthlyStats.Sum(s => s.TotalMileage).ToString("F0"));
                csv.WriteField(monthlyStats.Sum(s => s.RefuelCount));
                csv.WriteField(monthlyStats.Sum(s => s.MaintenanceCount));
                await csv.NextRecordAsync();

                return (true, $"Export des statistiques mensuelles réussi: {monthlyStats.Count} mois analysé(s).");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de l'export des statistiques mensuelles: {ex.Message}");
            }
        }

        #endregion

        #region Export PDF Améliorés

        /// <summary>
        /// Génère un rapport PDF professionnel avec mise en page améliorée
        /// </summary>
        public (bool Success, string Message) GeneratePdfReport(string title, string content, string filePath)
        {
            try
            {
                using var writer = new PdfWriter(filePath);
                using var pdf = new PdfDocument(writer);
                using var document = new Document(pdf);

                // Titre principal
                var titleParagraph = new Paragraph(title)
                    .SetFontSize(22)
                    
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(10);
                document.Add(titleParagraph);

                // Date et heure de génération
                var dateParagraph = new Paragraph($"Généré le {DateTime.Now:dd/MM/yyyy à HH:mm}")
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetMarginBottom(30);
                document.Add(dateParagraph);

                // Ligne de séparation
                document.Add(new Paragraph("_".PadRight(100, '_'))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20));

                // Contenu
                document.Add(new Paragraph(content)
                    .SetFontSize(12));

                // Footer
                document.Add(new Paragraph("\n\n" + "_".PadRight(100, '_'))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginTop(30));
                
                document.Add(new Paragraph($"Fleet Manager - Rapport généré automatiquement le {DateTime.Now:dd/MM/yyyy}")
                    .SetFontSize(9)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginTop(10));

                return (true, "Rapport PDF généré avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de la génération du PDF: {ex.Message}");
            }
        }

        /// <summary>
        /// Génère un rapport PDF avancé avec statistiques complètes et tableaux détaillés
        /// </summary>
        public (bool Success, string Message) GenerateAdvancedPdfReport(string title, FleetStatistics fleetStats, List<VehicleStatistics> vehicleStats, string filePath)
        {
            try
            {
                using var writer = new PdfWriter(filePath);
                using var pdf = new PdfDocument(writer);
                using var document = new Document(pdf);

                // En-tête avec logo et titre
                var titleParagraph = new Paragraph(title)
                    .SetFontSize(24)
                    
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(5);
                document.Add(titleParagraph);

                var subtitleParagraph = new Paragraph("Rapport d'Analyse de la Flotte")
                    .SetFontSize(14)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20);
                document.Add(subtitleParagraph);

                // Informations de génération
                var infoParagraph = new Paragraph()
                    .Add($"Date de génération: {DateTime.Now:dd/MM/yyyy HH:mm}\n")
                    .Add($"Période d'analyse: 12 derniers mois\n")
                    .Add($"Nombre de véhicules analysés: {vehicleStats.Count}")
                    .SetFontSize(10)
                    .SetMarginBottom(25);
                document.Add(infoParagraph);

                // Ligne de séparation
                document.Add(new Paragraph("═".PadRight(100, '═'))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20));

                // SECTION 1: Statistiques Globales
                document.Add(new Paragraph("📊 STATISTIQUES GLOBALES DE LA FLOTTE")
                    .SetFontSize(18)
                    
                    .SetMarginBottom(15));

                var statsTable = new Table(new float[] { 3, 2 }).UseAllAvailableWidth();
                statsTable.SetMarginBottom(20);

                AddStatRow(statsTable, "Nombre total de véhicules", fleetStats.TotalVehicles.ToString());
                AddStatRow(statsTable, "Véhicules actifs", $"{fleetStats.ActiveVehicles} ({(fleetStats.TotalVehicles > 0 ? (decimal)fleetStats.ActiveVehicles / fleetStats.TotalVehicles * 100 : 0):F1}%)");
                AddStatRow(statsTable, "Véhicules en maintenance", fleetStats.VehiclesInMaintenance.ToString());
                AddStatRow(statsTable, "Kilométrage total de la flotte", $"{fleetStats.TotalMileage:N0} km");
                AddStatRow(statsTable, "Consommation moyenne flotte", $"{fleetStats.AverageFleetConsumption:F2} L/100km");
                AddStatRow(statsTable, "Coût total carburant", $"{fleetStats.TotalFuelCost:C}");
                AddStatRow(statsTable, "Coût total maintenance", $"{fleetStats.TotalMaintenanceCost:C}");
                AddStatRow(statsTable, "Coût total d'exploitation", $"{fleetStats.TotalFuelCost + fleetStats.TotalMaintenanceCost:C}");
                AddStatRow(statsTable, "Coût mensuel moyen carburant", $"{fleetStats.MonthlyFuelCost:C}");
                AddStatRow(statsTable, "Coût mensuel moyen maintenance", $"{fleetStats.MonthlyMaintenanceCost:C}");

                document.Add(statsTable);

                // SECTION 2: Top 10 Véhicules par Coût
                document.Add(new Paragraph("\n💰 TOP 10 VÉHICULES PAR COÛT TOTAL")
                    .SetFontSize(18)
                    
                    .SetMarginBottom(15));

                var topCostTable = new Table(new float[] { 1, 2, 2, 2, 2, 2, 2 }).UseAllAvailableWidth();
                topCostTable.SetMarginBottom(20);

                // En-têtes
                AddHeaderCell(topCostTable, "#");
                AddHeaderCell(topCostTable, "Véhicule");
                AddHeaderCell(topCostTable, "Immatriculation");
                AddHeaderCell(topCostTable, "Conso.\n(L/100km)");
                AddHeaderCell(topCostTable, "Coût\nCarburant");
                AddHeaderCell(topCostTable, "Coût\nMaintenance");
                AddHeaderCell(topCostTable, "Coût\nTotal");

                int rank = 1;
                foreach (var vehicle in vehicleStats.OrderByDescending(v => v.TotalCost).Take(10))
                {
                    topCostTable.AddCell(new Cell().Add(new Paragraph(rank.ToString())).SetTextAlignment(TextAlignment.CENTER));
                    topCostTable.AddCell(new Cell().Add(new Paragraph(vehicle.VehicleName)));
                    topCostTable.AddCell(new Cell().Add(new Paragraph(vehicle.RegistrationNumber)).SetTextAlignment(TextAlignment.CENTER));
                    topCostTable.AddCell(new Cell().Add(new Paragraph($"{vehicle.AverageConsumption:F2}")).SetTextAlignment(TextAlignment.RIGHT));
                    topCostTable.AddCell(new Cell().Add(new Paragraph($"{vehicle.TotalFuelCost:C}")).SetTextAlignment(TextAlignment.RIGHT));
                    topCostTable.AddCell(new Cell().Add(new Paragraph($"{vehicle.TotalMaintenanceCost:C}")).SetTextAlignment(TextAlignment.RIGHT));
                    topCostTable.AddCell(new Cell().Add(new Paragraph($"{vehicle.TotalCost:C}")).SetTextAlignment(TextAlignment.RIGHT));
                    rank++;
                }

                document.Add(topCostTable);

                // SECTION 3: Top 10 Véhicules par Consommation
                document.Add(new Paragraph("\n⛽ TOP 10 VÉHICULES PAR CONSOMMATION")
                    .SetFontSize(18)
                    
                    .SetMarginBottom(15));

                var topConsumptionTable = new Table(new float[] { 1, 2, 2, 2, 2, 2 }).UseAllAvailableWidth();
                topConsumptionTable.SetMarginBottom(20);

                // En-têtes
                AddHeaderCell(topConsumptionTable, "#");
                AddHeaderCell(topConsumptionTable, "Véhicule");
                AddHeaderCell(topConsumptionTable, "Immatriculation");
                AddHeaderCell(topConsumptionTable, "Kilométrage");
                AddHeaderCell(topConsumptionTable, "Nb Pleins");
                AddHeaderCell(topConsumptionTable, "Conso.\n(L/100km)");

                rank = 1;
                foreach (var vehicle in vehicleStats.OrderByDescending(v => v.AverageConsumption).Take(10))
                {
                    topConsumptionTable.AddCell(new Cell().Add(new Paragraph(rank.ToString())).SetTextAlignment(TextAlignment.CENTER));
                    topConsumptionTable.AddCell(new Cell().Add(new Paragraph(vehicle.VehicleName)));
                    topConsumptionTable.AddCell(new Cell().Add(new Paragraph(vehicle.RegistrationNumber)).SetTextAlignment(TextAlignment.CENTER));
                    topConsumptionTable.AddCell(new Cell().Add(new Paragraph($"{vehicle.CurrentMileage:N0} km")).SetTextAlignment(TextAlignment.RIGHT));
                    topConsumptionTable.AddCell(new Cell().Add(new Paragraph(vehicle.TotalRefuels.ToString())).SetTextAlignment(TextAlignment.CENTER));
                    topConsumptionTable.AddCell(new Cell().Add(new Paragraph($"{vehicle.AverageConsumption:F2}")).SetTextAlignment(TextAlignment.RIGHT));
                    rank++;
                }

                document.Add(topConsumptionTable);

                // SECTION 4: Analyse par Coût par Kilomètre
                document.Add(new Paragraph("\n📈 ANALYSE PAR COÛT PAR KILOMÈTRE")
                    .SetFontSize(18)
                    
                    .SetMarginBottom(15));

                var costPerKmTable = new Table(new float[] { 1, 2, 2, 2, 2 }).UseAllAvailableWidth();
                costPerKmTable.SetMarginBottom(20);

                // En-têtes
                AddHeaderCell(costPerKmTable, "#");
                AddHeaderCell(costPerKmTable, "Véhicule");
                AddHeaderCell(costPerKmTable, "Kilométrage");
                AddHeaderCell(costPerKmTable, "Coût Total");
                AddHeaderCell(costPerKmTable, "€/km");

                rank = 1;
                foreach (var vehicle in vehicleStats.OrderByDescending(v => v.CostPerKilometer).Take(10))
                {
                    costPerKmTable.AddCell(new Cell().Add(new Paragraph(rank.ToString())).SetTextAlignment(TextAlignment.CENTER));
                    costPerKmTable.AddCell(new Cell().Add(new Paragraph(vehicle.VehicleName)));
                    costPerKmTable.AddCell(new Cell().Add(new Paragraph($"{vehicle.CurrentMileage:N0} km")).SetTextAlignment(TextAlignment.RIGHT));
                    costPerKmTable.AddCell(new Cell().Add(new Paragraph($"{vehicle.TotalCost:C}")).SetTextAlignment(TextAlignment.RIGHT));
                    costPerKmTable.AddCell(new Cell().Add(new Paragraph($"{vehicle.CostPerKilometer:C}")).SetTextAlignment(TextAlignment.RIGHT));
                    rank++;
                }

                document.Add(costPerKmTable);

                // SECTION 5: Recommandations
                document.Add(new Paragraph("\n💡 RECOMMANDATIONS")
                    .SetFontSize(18)
                    
                    .SetMarginBottom(15));

                var recommendations = GenerateRecommendations(fleetStats, vehicleStats);
                foreach (var recommendation in recommendations)
                {
                    document.Add(new Paragraph($"• {recommendation}")
                        .SetFontSize(11)
                        .SetMarginBottom(5));
                }

                // Footer
                document.Add(new Paragraph("\n\n" + "═".PadRight(100, '═'))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginTop(30));

                document.Add(new Paragraph($"Rapport généré par Fleet Manager - {DateTime.Now:dd/MM/yyyy à HH:mm}")
                    .SetFontSize(9)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginTop(10));

                document.Add(new Paragraph("Ce rapport est confidentiel et destiné uniquement à un usage interne.")
                    .SetFontSize(8)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginTop(5));

                return (true, "Rapport PDF avancé généré avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de la génération du PDF avancé: {ex.Message}");
            }
        }

        // Méthodes auxiliaires pour les PDF
        private void AddStatRow(Table table, string label, string value)
        {
            table.AddCell(new Cell().Add(new Paragraph(label)));
            table.AddCell(new Cell().Add(new Paragraph(value)).SetTextAlignment(TextAlignment.RIGHT));
        }

        private void AddHeaderCell(Table table, string text)
        {
            table.AddHeaderCell(new Cell().Add(new Paragraph(text)).SetTextAlignment(TextAlignment.CENTER));
        }

        private List<string> GenerateRecommendations(FleetStatistics fleetStats, List<VehicleStatistics> vehicleStats)
        {
            var recommendations = new List<string>();

            // Analyse de la consommation
            var highConsumptionVehicles = vehicleStats.Where(v => v.AverageConsumption > fleetStats.AverageFleetConsumption * 1.2m).Count();
            if (highConsumptionVehicles > 0)
            {
                recommendations.Add($"{highConsumptionVehicles} véhicule(s) ont une consommation supérieure de 20% à la moyenne. Envisager un diagnostic ou remplacement.");
            }

            // Analyse des coûts
            var averageCostPerKm = vehicleStats.Average(v => v.CostPerKilometer);
            var expensiveVehicles = vehicleStats.Where(v => v.CostPerKilometer > averageCostPerKm * 1.5m).Count();
            if (expensiveVehicles > 0)
            {
                recommendations.Add($"{expensiveVehicles} véhicule(s) ont un coût au kilomètre 50% supérieur à la moyenne. Analyser la rentabilité.");
            }

            // Maintenance
            var vehiclesNeedingMaintenance = vehicleStats.Where(v => v.DaysSinceLastMaintenance > 90).Count();
            if (vehiclesNeedingMaintenance > 0)
            {
                recommendations.Add($"{vehiclesNeedingMaintenance} véhicule(s) n'ont pas eu de maintenance depuis plus de 90 jours. Planifier une révision.");
            }

            // Ratio carburant/maintenance
            if (fleetStats.TotalMaintenanceCost > fleetStats.TotalFuelCost * 0.5m)
            {
                recommendations.Add("Les coûts de maintenance représentent plus de 50% des coûts carburant. Considérer le renouvellement de certains véhicules.");
            }

            // Si pas de recommandations
            if (recommendations.Count == 0)
            {
                recommendations.Add("La flotte est bien gérée. Continuer le suivi régulier des indicateurs.");
                recommendations.Add("Maintenir les bonnes pratiques de maintenance préventive.");
            }

            return recommendations;
        }

        #endregion

        #region Méthodes Utilitaires

        /// <summary>
        /// Calcule la note de performance d'un véhicule (A à F)
        /// </summary>
        private string CalculatePerformanceGrade(decimal consumption, decimal costPerKm)
        {
            // Système de notation basé sur la consommation et le coût
            var score = 0;

            // Score consommation (0-50 points)
            if (consumption < 5) score += 50;
            else if (consumption < 6) score += 40;
            else if (consumption < 7) score += 30;
            else if (consumption < 8) score += 20;
            else if (consumption < 10) score += 10;

            // Score coût/km (0-50 points)
            if (costPerKm < 0.15m) score += 50;
            else if (costPerKm < 0.20m) score += 40;
            else if (costPerKm < 0.25m) score += 30;
            else if (costPerKm < 0.30m) score += 20;
            else if (costPerKm < 0.40m) score += 10;

            // Conversion en note
            if (score >= 80) return "A";
            if (score >= 60) return "B";
            if (score >= 40) return "C";
            if (score >= 20) return "D";
            if (score >= 10) return "E";
            return "F";
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

        #endregion
    }
}
