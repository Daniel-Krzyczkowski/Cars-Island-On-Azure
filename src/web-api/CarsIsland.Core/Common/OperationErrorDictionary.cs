namespace CarsIsland.Core.Common
{
    public static class OperationErrorDictionary
    {
        public static class CarReservation
        {
            public static OperationError CarAlreadyReserved() =>
               new OperationError("Unfortunately the car was already reserved by another client in this specific term.");

            public static OperationError CarDoesNotExist() =>
               new OperationError("Unfortunately the car specified in the reservation does not exist in out catalog.");
        }
    }
}
