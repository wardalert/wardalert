# Ward Alert

A community web application for Ward 17, Lalitpur (Lukhunsi) residents and administrators to share updates, community trainings, and events.

## Overview

Ward Alert is built with ASP.NET Core (targeting .NET 8.0) and MySQL backend. It provides:
- Public pages for viewing ongoing/upcoming trainings and announcements
- Admin dashboard for managing content
- User registration with verification process
- Dynamic training schedule management
- Community event bulletins

## Key Features

### Community Home Page
- Landing page welcomes users to Ward 17 (Lalitpur, Lukhunsi)
- Navigation cards for Ongoing Trainings, Upcoming Trainings, and community updates
- Dynamic content pulled from database

### Training Management
- Public pages list trainings by status (Ongoing, Upcoming, Expired)
- Background service tracks training statuses based on dates:
  ```csharp
  DateTime now = DateTime.Now;
  if (now < startDate) return "Upcoming";
  if (now <= endDate) return "Ongoing";
  return "Expired";
  ```
- Helps residents stay informed of local training programs

### Event Announcements
- Events page displays community announcements from database
- Each event shows title, description, and upload date
- Provides a newsfeed of the latest ward notices

### User Registration & Verification
- New users register with personal information
- Upload citizenship ID images for verification
- Administrators can approve or decline users
- Ensures only verified community members access certain features

### Admin Dashboard
- Secure login system
- Tools to manage all content
- Create, edit, or delete trainings and events
- User management with approval workflow
- Session-based security

## Prerequisites

- .NET 8.0 SDK or Visual Studio 2022+
- MySQL Server
  - Default connection string: `Server=localhost;Database=project;User=root;Password=;`
  - Database named `project` (or adjust connection string)
- MySql.Data Connector (v9.2.0)
- (Optional) Code editor or IDE (Visual Studio, VS Code)

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/wardalert/wardalert.git
   cd wardalert
   ```

2. Restore and build:
   ```bash
   dotnet restore
   dotnet build
   ```

3. Configure the database:
   - Create MySQL database named `project` (or update connection strings)
   - Ensure these tables exist:
     - `training` (TrainingId, Title, Description, Status, Time, StartDate, EndDate, Capacity, ...)
     - `event` (id, Title, Description, UploadedAt, ...)
     - `user` (id, Name, Address, Phone, Email, Gender, Status, UploadedFile1, UploadedFile2, ...)

4. Run the application:
   ```bash
   dotnet run
   ```
   - Default URLs: http://localhost:5031 and https://localhost:7214

## Usage

### Public Access
- Browse site home page, view events, and check training lists
- Access Ongoing/Upcoming/Expired Trainings via home page links

### User Registration
- Click Register link to complete registration form
- Submit form and wait for admin approval

### Admin Access
- Navigate to `/Login`
- Default credentials: kri@gmail.com / krisha123
- After login, redirects to `/Admin/Dashboard`

### Admin Functions
- **Create Training**: Add new training events with details
- **Training List**: View, edit, or delete trainings
- **Create Event**: Add community announcements
- **User List**: Review and approve user registrations

### Configuration
- Edit `Properties/launchSettings.json` to change ports/URLs
- Update database credentials in code (currently hard-coded)

## Contribution

Contributions are welcome! Please:
- Fork the repository
- Submit pull requests for improvements or new features
- Ensure changes comply with the AGPL-3.0 license
- Include proper attribution
- Open an issue first to discuss major changes

## License

This project is licensed under the GNU Affero General Public License v3.0 (AGPL-3.0). This means:
- Source code is fully available
- Any distributed modifications must also be made public under the same license
- See the LICENSE file for details

## Usage Rights

This project, Ward Alert, is owned and maintained by Suraj Pathak, Sudipa Amgai, and Krisha Maharjan. This repository is licensed under the GNU Affero General Public License v3.0 (AGPL-3.0).

Under this license:
- The code is freely viewable
- Anyone who uses, modifies, or distributes this code must release their entire source code under the same AGPL-3.0 license
- This includes web applications and network services using this code
- All modifications must be shared with the community
- Proper attribution to the original authors is required

While you may fork this repository according to the terms of the AGPL-3.0 license, we ask that you respect our work and contact us before using this code in any projects.

© 2025 Suraj Pathak, Sudipa Amgai, Krisha Maharjan. All Rights Reserved.
