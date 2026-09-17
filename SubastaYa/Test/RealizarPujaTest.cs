using Application.DTOs.Puja;
using Application.Interfaces;
using Application.UseCases.Pujas;
using Xunit;
using Moq;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class RealizarPujaTest
    {
        [Fact]
        public async Task ExecuteAsync_VendedorIntentaPujarEnSuSubasta()
        {
            // 1. Arrange (Preparación)
            int subastaId = 1;
            int vendedorId = 10;

            // Creamos una subasta simulada donde el vendedor es el usuario ID 10
            var subastaMock = new Subasta
            (
                vendedorId: vendedorId,
                categoriaId: 1,             // Asigna el ID de categoría que corresponda
                titulo: "Subasta de prueba",
                descripcion: "Descripción",
                urlImagen: "http://...",
                precioInicial: 10000,
                incrementoMinimo: 1000,
                fechaInicio: DateTime.UtcNow.AddHours(-1),
                fechaFin: DateTime.UtcNow.AddHours(1)

            );

            var subastaRepoMock = new Mock<ISubastaRepository>();
            subastaRepoMock.Setup(r => r.GetByIdAsync(subastaId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(subastaMock);

            // Mocks vacíos para las demás dependencias que requiere el constructor
            var billeteraRepoMock = new Mock<IBilleteraRepository>();
            var pujaRepoMock = new Mock<IPujaRepository>();
            var auditoriaRepoMock = new Mock<IAuditoriaRepository>();
            auditoriaRepoMock.Setup(r => r.AddAsync(It.IsAny<AuditoriaLog>()))
                             .Returns(Task.CompletedTask);
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(Mock.Of<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>());
            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(1);
            var notificadorMock = new Mock<INotificadorSubasta>();

            // Instanciamos el caso de uso inyectando los mocks
            var useCase = new RealizarPuja(
                subastaRepoMock.Object,
                billeteraRepoMock.Object,
                pujaRepoMock.Object,
                auditoriaRepoMock.Object,
                unitOfWorkMock.Object,
                notificadorMock.Object
            );

            // DTO donde el CompradorId es el mismo que el VendedorId (10)
            var dto = new CrearPujaDto
            {
                CompradorId = 10,
                Monto = 15000
            };

            // 2. Act & 3. Assert (Ejecución y Verificación)
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                useCase.ExecuteAsync(subastaId, dto)
            );

            Assert.Contains("El vendedor no puede pujar", exception.Message);
        }

        [Fact]
        public async Task ExecuteAsync_ElMontoEsMenorAlMinimoRequerido()
        {
            // Arrange
            int vendedorId = 10;
            int subastaId = 1;
            var subastaMock = new Subasta
            (
                vendedorId: vendedorId,
                categoriaId: 1,             // Asigna el ID de categoría que corresponda
                titulo: "Subasta de prueba",
                descripcion: "Descripción",
                urlImagen: "http://...",
                precioInicial: 10000,
                incrementoMinimo: 1000,
                fechaInicio: DateTime.UtcNow.AddHours(-1),
                fechaFin: DateTime.UtcNow.AddHours(1)
            );

            var subastaRepoMock = new Mock<ISubastaRepository>();
            subastaRepoMock.Setup(r => r.GetByIdAsync(subastaId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(subastaMock);

            // Simulamos que ya existe una puja máxima de 20,000
            // Por lo tanto, el mínimo requerido debería ser 20,000 + 2,000 = 22,000
            var pujaAnterior = new Puja(subastaId, 8, 20000, DateTime.UtcNow);

            var pujaRepoMock = new Mock<IPujaRepository>();
            pujaRepoMock.Setup(r => r.GetUltimaPujaAsync(subastaId))
                        .ReturnsAsync(pujaAnterior);

            var billeteraRepoMock = new Mock<IBilleteraRepository>();

            // Creamos una billetera ficticia para el usuario 9 con saldo suficiente 
            // (ajusta los parámetros según el constructor de tu entidad Billetera)
            var billeteraMock = new Billetera(
            usuarioId: 9,
            saldoTotal: 50000,
            saldoRetenido: 0,
            version: 1
            );

            billeteraRepoMock.Setup(r => r.GetByUsuarioIdAsync(9))
                             .ReturnsAsync(billeteraMock);
            var auditoriaRepoMock = new Mock<IAuditoriaRepository>();
            auditoriaRepoMock.Setup(r => r.AddAsync(It.IsAny<AuditoriaLog>()))
                             .Returns(Task.CompletedTask);
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(Mock.Of<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>());
            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(1);
            var notificadorMock = new Mock<INotificadorSubasta>();

            var useCase = new RealizarPuja(
                subastaRepoMock.Object,
                billeteraRepoMock.Object,
                pujaRepoMock.Object,
                auditoriaRepoMock.Object,
                unitOfWorkMock.Object,
                notificadorMock.Object
            );

            // Enviamos un monto de 21,000 (menor al mínimo requerido de 22,000)
            var dto = new CrearPujaDto
            {
                CompradorId = 9,
                Monto = 20000
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                useCase.ExecuteAsync(subastaId, dto)
            );

            Assert.Contains("El monto debe ser al menos", exception.Message);
        }
    }
}
