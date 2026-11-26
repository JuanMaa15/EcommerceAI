using EcommerceAI.Domain.Enums;
using EcommerceAI.Domain.Exceptions;
using EcommerceAI.Domain.Interfaces;

namespace EcommerceAI.Domain.Entities;

public class UserToken : IEntity
{
    private const int TokenMaxLength = 512;

    // Duraciones fijas por tipo de token. Más adelante pueden moverse a configuración.
    private static readonly TimeSpan VerificationEmailLifetime = TimeSpan.FromMinutes(15);
    private static readonly TimeSpan RecoverPasswordLifetime = TimeSpan.FromMinutes(15);
    private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(15);

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public TokenType Type { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsUsed { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Constructor requerido por EF Core para materializar la entidad sin ejecutar validaciones.
    private UserToken()
    {
    }

    // El token debe llegar ya hasheado; la generación y el hash se realizan fuera del dominio.
    public UserToken(Guid userId, string token, TokenType type)
    {
        if (userId == Guid.Empty)
            throw new DomainException("El token debe pertenecer a un usuario.");

        if (string.IsNullOrWhiteSpace(token))
            throw new DomainException("El token no puede estar vacío.");

        if (token.Length > TokenMaxLength)
            throw new DomainException($"El token no puede superar los {TokenMaxLength} caracteres.");

        if (!Enum.IsDefined(type))
            throw new DomainException("El tipo de token especificado no es válido.");

        Id = Guid.NewGuid();
        UserId = userId;
        Token = token;
        Type = type;
        IsActive = true;
        IsUsed = false;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = CreatedAt.Add(GetLifetime(type));
        UpdatedAt = null;
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow >= ExpiresAt;
    }

    public bool IsValid()
    {
        return IsActive && !IsUsed && !IsExpired();
    }

    public void MarkAsUsed()
    {
        if (!IsActive)
            throw new DomainException("El token ha sido revocado.");

        if (IsUsed)
            throw new DomainException("El token ya fue utilizado.");

        if (IsExpired())
            throw new DomainException("El token ha expirado.");

        IsUsed = true;
        UpdatedAt = DateTime.UtcNow;
    }

    // Reactivar un token revocado reabriría, por ejemplo, una sesión cerrada por el usuario.
    public void Activate()
    {
        if (IsActive)
            throw new DomainException("El token ya está activo.");

        throw new DomainException("Un token revocado no puede reactivarse.");
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private static TimeSpan GetLifetime(TokenType type)
    {
        return type switch
        {
            TokenType.VerificationEmail => VerificationEmailLifetime,
            TokenType.RecoverPassword => RecoverPasswordLifetime,
            TokenType.RefreshToken => RefreshTokenLifetime,
            _ => throw new DomainException("El tipo de token especificado no es válido.")
        };
    }
}
