using System.Text.RegularExpressions;
using EcommerceAI.Domain.Enums;
using EcommerceAI.Domain.Exceptions;

namespace EcommerceAI.Domain.Entities;

public partial class User
{
    private const int NameMinLength = 2;
    private const int NameMaxLength = 50;
    private const int EmailMaxLength = 75;

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string? Phone { get; private set; }
    public UserRole Role { get; private set; }
    public bool VerificationEmail { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? UpdatedEmailAt { get; private set; }
    public DateTime? UpdatedPasswordAt { get; private set; }

    // Constructor requerido por EF Core para materializar la entidad sin ejecutar validaciones.
    private User()
    {
    }

    public User(string name, string lastName, string email, string password, string? phone = null, UserRole role = UserRole.Client)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetLastName(lastName);
        SetEmail(email);
        SetPassword(password);
        SetPhone(phone);
        SetRole(role);
        VerificationEmail = false;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
        UpdatedEmailAt = null;
        UpdatedPasswordAt = null;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("El nombre no puede estar vacío.");

        if (name.Length < NameMinLength || name.Length > NameMaxLength)
            throw new DomainException($"El nombre debe tener entre {NameMinLength} y {NameMaxLength} caracteres.");

        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("El apellido no puede estar vacío.");

        if (lastName.Length < NameMinLength || lastName.Length > NameMaxLength)
            throw new DomainException($"El apellido debe tener entre {NameMinLength} y {NameMaxLength} caracteres.");

        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("El email no puede estar vacío.");

        email = email.Trim().ToLowerInvariant();

        if (email.Length > EmailMaxLength)
            throw new DomainException($"El email no puede superar los {EmailMaxLength} caracteres.");

        if (!EmailRegex().IsMatch(email))
            throw new DomainException("El email no tiene un formato válido.");

        Email = email;
        VerificationEmail = false;
        UpdatedEmailAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new DomainException("La contraseña no puede estar vacía.");

        Password = password;
        UpdatedPasswordAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPhone(string? phone)
    {
        if (phone is not null && string.IsNullOrWhiteSpace(phone))
            throw new DomainException("El teléfono no puede ser una cadena vacía.");

        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetRole(UserRole role)
    {
        if (!Enum.IsDefined(role))
            throw new DomainException("El rol especificado no es válido.");

        Role = role;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void VerifyEmail()
    {
        VerificationEmail = true;
        UpdatedAt = DateTime.UtcNow;
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$")]
    private static partial Regex EmailRegex();
}
