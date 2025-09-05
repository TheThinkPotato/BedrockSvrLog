import { Box } from "@mui/material";
import { useRef } from "react";
import WorldClock from "../WorldClock";
import OnlineUserIndicator from "../OnlineUserIndicator";
import RefreshButton from "../RefreshButton";

interface HomeScreenProps {
  showSeedMap: boolean;
  seed?: string;
}

const HomeScreen = ({ showSeedMap, seed }: HomeScreenProps) => {
  const localMapRef = useRef<HTMLIFrameElement>(null);
  const seedMapRef = useRef<HTMLIFrameElement>(null);

  const refreshIframe = () => {

    if (seedMapRef.current && showSeedMap) {
      const currentSrc = seedMapRef.current.src;
      seedMapRef.current.src = "";
      setTimeout(() => {
        if (seedMapRef.current) {
          seedMapRef.current.src = currentSrc;
        }
      }, 100);
    }

    if (localMapRef.current && !showSeedMap) {
      const currentSrc = localMapRef.current.src;
      localMapRef.current.src = "";
      setTimeout(() => {
        if (localMapRef.current) {
          localMapRef.current.src = currentSrc;
        }
      }, 100);
    }
  };

  return (
    <Box className="w-full h-full p-4">
      <Box className="w-full h-full rounded-lg overflow-hidden border border-gray-700 relative">
        <WorldClock showSeedMap={showSeedMap} />
        <OnlineUserIndicator showSeedMap={showSeedMap} />
        <RefreshButton refreshIframe={refreshIframe} showSeedMap={showSeedMap} />

        <iframe
        style={{ visibility: !showSeedMap ? "visible" : "hidden" }}
          ref={localMapRef}
          src="map/index.html"
          title="Bedrock Server Map - Local"
          className={`w-full h-full border-0 absolute top-0 left-0 ${
            !showSeedMap ? "z-10" : "z-0"
          }`}
          sandbox="allow-same-origin allow-scripts allow-forms allow-popups allow-popups-to-escape-sandbox"
        />

        <iframe
          style={{ visibility: showSeedMap ? "visible" : "hidden" }}
          ref={seedMapRef}
          src={`https://mcseedmap.net/1.21.60-Bedrock/${seed}`}
          title="Bedrock Server Map - Seed"
          className={`w-full h-full border-0 absolute top-0 left-0 ${
            showSeedMap ? "z-10" : "z-0"
          }`}
          sandbox="allow-same-origin allow-scripts allow-forms allow-popups allow-popups-to-escape-sandbox"
        />
      </Box>
    </Box>
  );
};

export default HomeScreen;
