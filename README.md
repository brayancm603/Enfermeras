# Manual Técnico

## 1. Introducción:
Top services es una aplicación web innovadora que está diseñada para que lo usen desde cualquier departamento de Bolivia, cuyo principal propósito es de poder brindar servicios que requieran una ayuda profesional. Los usuarios podrán solicitar un servicio dependiendo a    su problema al que necesite ayuda. Podrán solicitar al profesional que este registrado en el sistema y de esta forma poder hacer la cotización de lo que costara el servicio.
## 2. Descripción del proyecto:
Top services es una solución tecnológica robusta y de fácil manejo, diseñada para poder facilitar la ayuda de cualquier servicio profesional que requiera un cliente en un respectivo departamento de Bolivia, los profesionales podrán realizar su postulación desde el sistema y luego sacar cita para el respectivo tramite, los administradores tendrán que ser registrados por el super administrador para todos los departamentos que requieran.
## 3. Arquitectura del software: Explicación de la estructa
La aplicación está construida sobre ASP.Net MVC, un framework que nos proporciona una arquitectura Modelo Vista Controlador, de esta manera nos garantiza que la aplicación tenga una estructura sólida y optima, y a futuro tener un mantenimiento exitoso, usamos la tecnología de Boostrap que nos ayudó a darle estilos a las paginas y darle un renderizado agradable a la vista del usuario dándonos plantillas para que los desarrolladores puedan usarlos directamente. De la misma forma se usa Firebase para el almacenamiento de las imágenes en la nube.
## 4. Características Técnicas
- 🪄 ASP.Net MVC
- 🎨 Boostrap
- 🛢️ SQLServer
- 🔥 FireBase
## 5. Comenzando
### 1. Clonar el repositorio
   
  ```bash
   git clone https://github.com/brayancm603/Enfermeras.git
 ```
### 2. Instalaciones
Para poder hacer la depuración del proyecto necesitaremos de dos técnologias que son Visual Studio 2022 que se puede descargar de esta pagina (https://visualstudio.microsoft.com/es/downloads/) y tambien necesitaremos instalar el motor de la base de datos en este caso SQLServer que lo podra descargar de esta pagina web (https://www.microsoft.com/es-es/sql-server/sql-server-downloads) tambien le proporcionara un idle que es el managment studio 2019 que le podra ayudar a interactuar con el motor de BD si es que lo desea.

### 3. Restauración de la BD
Una vez que tenga el **.bak** de la BD necesitamos restaurarlo para poder usar los diferentes datos y usuarios registrados por roles, esto lo podemos hacer a travez del managment studio 2019 o si lo desea desde consola entrando al servidor/servidor local, para mayor información de como restaurar la BD puede visitar esta página web (https://learn.microsoft.com/es-es/sql/relational-databases/backup-restore/quickstart-backup-restore-database?view=sql-server-ver16&tabs=ssms)

### 4. Ejecución del proyecto 
Una vez que realize la clonación del proyecto procedemos a abrirlo y encontraremos el ejecutable del proyecto. Procedemos a abrirlo
![Ejecutador](https://github.com/brayancm603/Enfermeras/assets/90205529/3383a1b5-ce87-4e85-a2f9-0df7b19e65f7)

### 5. Depuracion del proyecto 
Antes de realizar la corrida del proyecto necesitamos realizar los respectivos cambios a la cadena de conéxion a la base de datos, que este se encuentra en el archivo **appsettings.json**, cuando lo hábra necesitara escribir el nombre del servidor, user y contraseña correspondiente al servidor donde se encuentra la BD.
![Captura de pantalla 2023-11-23 021604](https://github.com/brayancm603/Enfermeras/assets/90205529/0c0ebc83-3c81-4f59-a6a8-68d35d07d437)
Déspues de realizar la modificación procedemos a depurar el proyecto, precionando el botón de depuracion del idle de visual studio
![Captura de pantalla 2023-11-23 021619](https://github.com/brayancm603/Enfermeras/assets/90205529/2771c4c8-ea64-4334-b38d-63513645db73)

## 6. Manual de usuaro 

