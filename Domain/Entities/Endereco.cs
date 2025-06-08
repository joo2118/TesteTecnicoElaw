namespace Domain.Entities
{
    public class Endereco
    {

        public Endereco(string rua, string numero, string cidade, string estado, string cep)
        {
            Rua = !string.IsNullOrWhiteSpace(rua) ? rua : throw new ArgumentException($"{nameof(rua)} não pode ser nulo, vazio ou em branco.", nameof(rua));
            Numero = !string.IsNullOrWhiteSpace(numero) ? numero : throw new ArgumentException($"{nameof(numero)} não pode ser nulo, vazio ou em branco.", nameof(numero));
            Cidade = !string.IsNullOrWhiteSpace(cidade) ? cidade : throw new ArgumentException($"{nameof(cidade)} não pode ser nulo, vazio ou em branco.", nameof(cidade));
            Estado = !string.IsNullOrWhiteSpace(estado) ? estado : throw new ArgumentException($"{nameof(estado)} não pode ser nulo, vazio ou em branco.", nameof(estado));
            CEP = !string.IsNullOrWhiteSpace(cep) ? cep : throw new ArgumentException($"{nameof(cep)} não pode ser nulo, vazio ou em branco.", nameof(cep));
        }

        protected Endereco() { }

        public string? Rua { get; protected set; }
        public string? Numero { get; protected set; }
        public string? Cidade { get; protected set; }
        public string? Estado { get; protected set; }
        public string? CEP { get; protected set; }
    }
}
