namespace SmartShip.ShipmentService.Domain.Entities
{
   
        public enum ShipmentStatus
        {
            Draft,
            Booked,
            PickedUp,
            InTransit,
            OutForDelivery,
            Delivered,
            Delayed,
            Failed,
            Returned
        }
    
}
