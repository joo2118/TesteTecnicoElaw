namespace Application.DTOs
{
    public class ClienteDTO
    {
        public ClienteDTO(Guid? id, string nome, string email, EnderecoDTO endereco, string? telefone = null)
        {
            Id = (id == null || id == Guid.Empty) ? Guid.NewGuid() : id;
            Nome = !string.IsNullOrWhiteSpace(nome) ? nome : throw new ArgumentException($"{nameof(nome)} não pode ser nulo, vazio ou em branco.", nameof(nome));
            Email = !string.IsNullOrWhiteSpace(email) ? email : throw new ArgumentException($"{nameof(email)} não pode ser nulo, vazio ou em branco.", nameof(email));
            Endereco = endereco ?? throw new ArgumentNullException(nameof(endereco));
            Telefone = telefone;
        }

        public Guid? Id { get; set; }
        public string Nome { get; }
        public string Email { get; }
        public EnderecoDTO Endereco { get; }
        public string? Telefone { get; }
    }
}
