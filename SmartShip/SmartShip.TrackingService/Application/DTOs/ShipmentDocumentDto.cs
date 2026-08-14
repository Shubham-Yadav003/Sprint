namespace SmartShip.TrackingService.Application.DTOs
{
  public class ShipmentDocumentDto
        {
            public int Id { get; set; }

            public int ShipmentId { get; set; }

            public string DocumentType { get; set; } = string.Empty;

            public string FileName { get; set; } = string.Empty;

            public string FilePath { get; set; } = string.Empty;

            public DateTime UploadedAt { get; set; }
        }
    }
