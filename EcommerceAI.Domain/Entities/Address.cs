using EcommerceAI.Domain.Exceptions;

namespace EcommerceAI.Domain.Entities;

public class Address
{
    private const int AddressLineMaxLength = 200;
    private const int LocationMaxLength = 100;
    private const int ReferenceMaxLength = 250;
    private const int PhoneMaxLength = 20;

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string AddressLine { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string Province { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public string? Reference { get; private set; }
    public string Phone { get; private set; } = string.Empty;
    public bool IsDefault { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Constructor requerido por EF Core para materializar la entidad sin ejecutar validaciones.
    private Address()
    {
    }

    // Solo el agregado User puede crear direcciones para garantizar la regla de dirección por defecto.
    internal Address(Guid userId, string addressLine, string city, string province, string country, string phone, string? reference = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        SetAddressLine(addressLine);
        SetCity(city);
        SetProvince(province);
        SetCountry(country);
        SetPhone(phone);
        SetReference(reference);
        IsDefault = false;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public void SetAddressLine(string addressLine)
    {
        AddressLine = ValidateRequired(addressLine, "La dirección", AddressLineMaxLength);
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCity(string city)
    {
        City = ValidateRequired(city, "La ciudad", LocationMaxLength);
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetProvince(string province)
    {
        Province = ValidateRequired(province, "La provincia", LocationMaxLength);
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCountry(string country)
    {
        Country = ValidateRequired(country, "El país", LocationMaxLength);
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPhone(string phone)
    {
        Phone = ValidateRequired(phone, "El teléfono", PhoneMaxLength);
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetReference(string? reference)
    {
        if (reference is not null && string.IsNullOrWhiteSpace(reference))
            throw new DomainException("La referencia no puede ser una cadena vacía.");

        if (reference is not null && reference.Length > ReferenceMaxLength)
            throw new DomainException($"La referencia no puede superar los {ReferenceMaxLength} caracteres.");

        Reference = reference;
        UpdatedAt = DateTime.UtcNow;
    }

    internal void MarkAsDefault()
    {
        if (!IsActive)
            throw new DomainException("Una dirección inactiva no puede ser la dirección por defecto.");

        IsDefault = true;
        UpdatedAt = DateTime.UtcNow;
    }

    internal void UnmarkAsDefault()
    {
        IsDefault = false;
        UpdatedAt = DateTime.UtcNow;
    }

    internal void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    internal void Deactivate()
    {
        if (IsDefault)
            throw new DomainException("No se puede desactivar la dirección por defecto. Primero marque otra dirección como predeterminada.");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private static string ValidateRequired(string value, string fieldName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException($"{fieldName} no puede estar vacío.");

        if (value.Length > maxLength)
            throw new DomainException($"{fieldName} no puede superar los {maxLength} caracteres.");

        return value;
    }
}
