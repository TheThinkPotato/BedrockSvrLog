# Bedrock Server Log WebApp

A React TypeScript application for monitoring Bedrock server statistics with an embedded map interface and real-time data displays.

## Features

- **Home Screen**: Embedded iframe for server map visualization
- **User Statistics**: Drawer panel with player overview and activity tables
- **World Statistics**: Drawer panel with dimension overview and event logs
- **Right-Side Menu**: Quick access navigation with three main sections
- **Modern UI**: Material-UI components with Tailwind CSS styling
- **Data Tables**: PrimeReact tables for displaying statistics

## Tech Stack

- **Frontend**: React 19 + TypeScript
- **Build Tool**: Vite
- **Package Manager**: pnpm
- **UI Components**: Material-UI (MUI)
- **Styling**: Tailwind CSS
- **Tables**: PrimeReact
- **Data Fetching**: React Query (TanStack Query)
- **Theme**: Dark mode with custom Material-UI theme

## Getting Started

### Prerequisites

- Node.js (v18 or higher)
- pnpm package manager

### Installation

1. Clone the repository:
```bash
git clone <repository-url>
cd bedrock-svr-log
```

2. Install dependencies:
```bash
pnpm install
```

3. Start the development server:
```bash
pnpm dev
```

4. Open your browser and navigate to `http://localhost:5173`

## Project Structure

```
src/
├── components/
│   ├── HomeScreen.tsx          # Main iframe container
│   ├── UserStatsDrawer.tsx     # User statistics drawer
│   └── WorldStatsDrawer.tsx    # World statistics drawer
├── App.tsx                     # Main application component
├── main.tsx                    # Application entry point
└── index.css                   # Global styles and imports
```

## Usage

### Navigation

- **Map Icon (Blue)**: Returns to the home screen with embedded iframe
- **User Icon (Gray/Green)**: Opens user statistics drawer
- **World Icon (Gray/Green)**: Opens world statistics drawer

### Drawers

Both statistics drawers slide in from the right side and can be closed by:
- Clicking the close button (X)
- Clicking outside the drawer
- Clicking another menu icon

### Mock Data

The application currently uses mock data for demonstration purposes. In a production environment, you would:
- Replace the mock data with real API calls
- Implement React Query for data fetching and caching
- Add real-time updates using WebSocket connections

## Customization

### Changing the Iframe Source

Edit `src/components/HomeScreen.tsx` and update the `src` attribute of the iframe:

```tsx
<iframe
  src="https://your-map-url.com"  // Change this to your actual map URL
  title="Bedrock Server Map"
  className="w-full h-full border-0"
  sandbox="allow-same-origin allow-scripts allow-forms allow-popups allow-popups-to-escape-sandbox"
/>
```

### Adding Real Data Sources

1. Create API service functions in a new `services/` directory
2. Use React Query hooks for data fetching
3. Replace the mock data in the drawer components

### Styling

- **Tailwind CSS**: Use utility classes for layout and spacing
- **Material-UI**: Use MUI components for interactive elements
- **Custom CSS**: Add custom styles in `index.css` or component-specific files

## Building for Production

```bash
pnpm build
```

The built files will be in the `dist/` directory.

## Development

```bash
# Start development server
pnpm dev

# Build for production
pnpm build

# Preview production build
pnpm preview

# Lint code
pnpm lint
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is licensed under the MIT License.
