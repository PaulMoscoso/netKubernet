namespace NetKubernetes.Dtos.InmuebleDtos
{
    public class InmuebleResponseDto
    {
        public int Id { get; set; }
        public string? Nombres { get; set; }
        public string? Direccion { get; set; }
        public decimal? Precio { get; set; }
        public string? Pictures { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
