namespace Domain.Entities
{
    public class Cliente
    {
        public Cliente(Guid id, string nome, string email, Endereco endereco, string? telefone = null)
        {
            Id = id != Guid.Empty ? id : throw new ArgumentException($"{nameof(id)} cannot be empty.", nameof(id));
            Nome = !string.IsNullOrWhiteSpace(nome) ? nome : throw new ArgumentException($"{nameof(nome)} não pode ser nulo, vazio ou em branco.", nameof(nome));
            Email = !string.IsNullOrWhiteSpace(email) ? email : throw new ArgumentException($"{nameof(email)} não pode ser nulo, vazio ou em branco.", nameof(email));
            Endereco = endereco ?? throw new ArgumentNullException(nameof(endereco));
            Telefone = telefone;
        }

        protected Cliente() { }

        public Guid Id { get; protected set; }
        public string? Nome { get; protected set; }
        public string? Email { get; protected set; }
        public Endereco? Endereco { get; protected set; }
        public string? Telefone { get; protected set; }
    }
}
