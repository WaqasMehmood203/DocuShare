namespace DMS.Backend.API.Service.Dtos.ExternalStorages
{
    public class ExternalStorageParametersDto : ParametersDto
    {
        public Guid? UserId { get; set; }
        public DateTime? ExpiryBefore { get; set; }
    }
}
