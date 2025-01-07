# KoçCo Project Overview

![image](https://github.com/user-attachments/assets/8ebf250f-0b09-45ec-806a-e9587d8b9c39)


## About the Repository
The backend architecture of the KoçCo project was entirely developed by myself, covering aspects such as the design of the database schema, API endpoints, and implementation of key business logic using Onion Architecture. The project includes robust authentication mechanisms, role-based access control, and seamless integration with the frontend. By focusing on scalable and maintainable code, the backend ensures the platform's long-term success and reliability.

### Features
- *Student Tracking and Performance Analysis*: Teachers can monitor students' progress, assign resources, and create customized study plans.
- *Integrated Messaging*: A real-time messaging system for seamless communication between students and teachers.
- *Resource Sharing*: Teachers can upload and share learning materials with their students.
- *Purchase and Management of Courses*: Students can easily purchase courses and view their enrolled lessons.
- *Feedback and Support*: Students can provide feedback and access support for a better user experience.


### Teacher Section
1. Teacher’s homepage upon login, displaying essential tools and metrics.
2. Teacher’s menu for navigation across different functionalities.
3. Profile page showcasing teacher’s personal details.
4. Profile editing page for updating information.
5. List of students assigned to the teacher.
6. Detailed views of a selected student’s progress and assignments.
7. Resources assigned to students by the teacher.
8. Messaging interface for teacher-student communication.

### Student Section
1. Login page for student users.
2. Password reset screens.
3. Student’s menu for navigating the app.
4. Homepage for students displaying their study progress.
5. Course purchase page.
6. "My Courses" page listing enrolled lessons.
7. Profile editing screen for students.
8. Page where students view available teachers.
9. Detailed view of a selected teacher’s profile.
10. Weekly plan provided by the teacher.
11. Messaging interface for student-teacher communication.
12. Page displaying the topics covered by the teacher.
13. Page listing courses purchased by the student.
14. Information about the application and its developers.
15. Feedback submission page.

## Key Backend Components
1. Authentication process for login and registration.

![image](https://github.com/user-attachments/assets/e22b7a63-3ebc-4413-b712-f0b1075239bf)



2. Features for managing payment cart: add, view, purchase, and delete.

![image](https://github.com/user-attachments/assets/4ff239c9-635a-40aa-b8b0-e708816c5183)


3. Features for Packages

![image](https://github.com/user-attachments/assets/1b144ad6-69d0-4e70-acb8-74a0794aef96)


4. Teacher’s interface for viewing/updating personal details, income tracking, uploading files, creating lesson schedules, and viewing test results.
 Student’s interface for viewing personal details, teacher-shared resources, downloading documents, and tracking test results.

![image](https://github.com/user-attachments/assets/58b59620-4ece-4cfd-97c5-7f5f8c1c0528)


##BAKCEND ARCHITECTURE
##ONION ARCHITECTURE
KoçCo's backend system is designed to handle complex business logic efficiently while maintaining scalability and security. By adhering to Onion Architecture,
the project ensures a clean separation of concerns, making it easy to extend and maintain in the future. 

![image](https://github.com/user-attachments/assets/26ebca7c-dbe1-4a0e-9c10-ff059bcd4c77)


##Project Structure

The project follows Onion Architecture to ensure a clean separation of concerns, promoting loose coupling and high cohesion.

##Layers of the Architecture

##Core Layer (Domain)
Entities: Represents the core objects in the system (e.g., User, Package, TestResult).
Interfaces: Defines contracts for repositories and services.
Services: Contains core domain services that implement business rules.

##Application Layer
DTOs: Used to transfer data between layers without exposing internal entities.
Interfaces: Defines service contracts that the controllers interact with.
Services: Implements application logic by coordinating with the domain layer.

##Infrastructure Layer
Persistence: Contains the database context (e.g., SqlDbContext) and migration configurations.
Repositories: Implements data access logic using Entity Framework Core.
Services: Handles external services such as payment gateways, email services, etc.

##API Layer
Controllers: Defines endpoints for the API.
Mapping: Uses AutoMapper to map between DTOs and domain entities.

![image](https://github.com/user-attachments/assets/e82a6d03-8841-4dad-b8c9-e1f3567b800c)

![image](https://github.com/user-attachments/assets/bdd525c7-c1cd-4d7c-9802-b8dacf547dc0)

![image](https://github.com/user-attachments/assets/f094bfe5-ed83-463e-b510-ef55c710c941)



## Web Application
The web platform provides additional capabilities for both students and teachers, maintaining consistency with the mobile application.
## Mobile Application
The mobile application provides distinct interfaces for students and teachers, ensuring functionality aligns with their roles.


## Technologies Used
- *Backend*: C# ASP.NET Core (API-driven architecture -  (THIS REPOSITORY)).
- *Frontend*: React (Web) and Flutter (Mobile).
- *Database*: SQL Server for robust data management.
- *Architecture*: Onion Architecture for maintaining scalability and separation of concerns. (THIS REPOSITORY)
