\# Backend API - SubastaYa



Este repositorio corresponde al backend (API REST) para el trabajo práctico.





\## ⚙️ Paso a Paso para Levantar el Proyecto



\### 1. Clonar el repositorio y abrir la terminal

Abre tu terminal y sitúate en la carpeta raíz del proyecto backend descargado.



\### 2. Restaurar las dependencias

Ejecuta el siguiente comando para descargar e instalar los paquetes NuGet necesarios:



dotnet restore



3\. Configurar la Base de Datos (Cadena de Conexión)



Abre el archivo appsettings.json que se encuentra en la raíz del proyecto.



Busca la sección "ConnectionStrings" y verifica que la cadena de conexión (DefaultConnection) apunte a tu servidor local de SQL Server utilizando la base de datos Subasta:



"ConnectionStrings": {

&#x20; "DefaultConnection": "Server=localhost;Database=Subasta;Trusted\_Connection=True;MultipleActiveResultSets=true"

}



4\. Aplicar Migraciones (Crear la Base de Datos automáticamente)

El proyecto utiliza Entity Framework Core. Para que las tablas se creen solas en tu base de datos local, ejecuta:



dotnet ef database update



(Nota: Si la consola te indica que no reconoce el comando dotnet ef, puedes instalar la herramienta globalmente ejecutando: dotnet tool install --global dotnet-ef).



5\. Compilar y Ejecutar la Aplicación

Inicia el servidor local de la API ejecutando:



dotnet run --project API



\-------------------------------------------------------------------------



Script de Prueba con Bash y curl



Puedes simular el envío concurrente de dos solicitudes idénticas ejecutando el siguiente bloque de comandos en la terminal de Linux, macOS o Git Bash en Windows:



\#!/bin/bash



\# URL del endpoint de pujas de tu API

URL="http://localhost:55976/api/Pujas"



\# Token JWT de autorización (reemplaza con un token válido de tu sistema)

TOKEN="TuTokenJwtValidoAqui"



\# JSON con los datos de la puja a enviar de forma idéntica

PAYLOAD='{"subastaId": 1, "monto": 15000}'



echo "🚀 Enviando dos peticiones de puja idénticas concurrentes..."



\# Lanzamos ambas peticiones en segundo plano (\&) de manera simultánea

curl -s -o /dev/null -w "Petición 1 - Código HTTP: %{http\_code}\\n" \\

&#x20; -X POST "$URL" \\

&#x20; -H "Content-Type: application/json" \\

&#x20; -H "Authorization: Bearer $TOKEN" \\

&#x20; -d "$PAYLOAD" \&



curl -s -o /dev/null -w "Petición 2 - Código HTTP: %{http\_code}\\n" \\

&#x20; -X POST "$URL" \\

&#x20; -H "Content-Type: application/json" \\

&#x20; -H "Authorization: Bearer $TOKEN" \\

&#x20; -d "$PAYLOAD" \&



\# Esperamos a que terminen ambos procesos en segundo plano

wait



echo "✅ Prueba finalizada. Una debió retornar éxito (200/201) y la otra conflicto (409)."

