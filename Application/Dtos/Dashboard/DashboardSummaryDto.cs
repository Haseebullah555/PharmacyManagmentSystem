namespace Application.Dtos.Dashboard
{
    public class DashboardSummaryDto
    {
        public decimal TodaySales { get; set; }
        public decimal TodayPurchases { get; set; }
        public int TodaySaleCount { get; set; }
        public int TodayPurchaseCount { get; set; }
        public int MedicinesInStock { get; set; }
        public decimal TotalStockQuantity { get; set; }
        public int ExpiringSoonCount { get; set; }
        public int ExpiredMedicineCount { get; set; }
        public List<DashboardDailyTotalDto> DailySales { get; set; } = new();
        public List<DashboardExpiryAlertDto> ExpiryAlerts { get; set; } = new();
    }

    public class DashboardDailyTotalDto
    {
        public string Date { get; set; } = string.Empty;
        public decimal Sales { get; set; }
        public decimal Purchases { get; set; }
    }

    public class DashboardExpiryAlertDto
    {
        public string MedicineName { get; set; } = string.Empty;
        public DateOnly? ExpiryDate { get; set; }
        public decimal Quantity { get; set; }
        public bool IsExpired { get; set; }
    }
}
