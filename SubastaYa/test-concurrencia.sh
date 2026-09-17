#!/bin/bash

# URL apuntando a tu endpoint exacto
URL="http://localhost:55976/api/subastas/16/pujas"

# ⚠️ PEGA AQUÍ TU TOKEN REAL COPIADO DE LOCALSTORAGE
TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2ZW5kZWRvckB0ZXN0LmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJWZW5kZWRvciBUZXN0IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImV4cCI6MTc4OTY0NDE3OSwiaXNzIjoiU3ViYXN0YVlhQVBJIiwiYXVkIjoiU3ViYXN0YVlhQ2xpZW50cyJ9.i94xGZFyeBiwrs_5EttIWkatpNbNvoLeoMhSYIenB9o"

# El body mapea exactamente con CrearPujaRequest (que espera 'monto')
PAYLOAD='{"monto": 150000}'

echo "🚀 Enviando dos peticiones de puja idénticas concurrentes..."

# Petición 1
curl -s -o /dev/null -w "%{http_code}" \
  -X POST "$URL" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d "$PAYLOAD" > res1.txt &

# Petición 2 (idéntica y enviada en paralelo)
curl -s -o /dev/null -w "%{http_code}" \
  -X POST "$URL" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d "$PAYLOAD" > res2.txt &

# Esperamos que terminen ambas
wait

# Resultados
echo "----------------------------------------"
echo "Petición 1 - Código HTTP: $(cat res1.txt)"
echo "Petición 2 - Código HTTP: $(cat res2.txt)"
echo "----------------------------------------"

# Limpieza
rm -f res1.txt res2.txt

echo "✅ Prueba finalizada."

