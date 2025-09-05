import { useEffect, useRef, useState } from "react";
import { Box } from "@mui/material";
import {
  minecraftTicksToTime,
  minecraftTimeDayConvert,
} from "../Helpers/timeHelper";
import { useWorldData } from "../context/WorldContext";
import WorldModal from "./Modals/WorldModal/WorldModal";

const WorldClock = ({ showSeedMap }: { showSeedMap: boolean }) => {
  const { lastMessage } = useWorldData();
  const [shownTime, setShownTime] = useState<number>(NaN);

  const {
    ticks: currentTicks,
    isDayIcon,
    timeColor,
  } = minecraftTimeDayConvert(lastMessage?.CurrentTime ?? NaN);

  const [modalWorldOpen, setModalWorldOpen] = useState(false);
  const prevTicks = useRef<number>(currentTicks);

  // Update shownTime every 250ms
  useEffect(() => {
    const interval = setInterval(() => {
      if (isNaN(shownTime) || shownTime === 0) {
        setShownTime(currentTicks);
      }

      if (currentTicks !== prevTicks.current) {
        prevTicks.current = currentTicks;
        setShownTime(currentTicks);
      }

      if (shownTime !== 0) {
        setShownTime((prev) => prev + 5);
      }
    }, 255);

    return () => clearInterval(interval);
  }, [currentTicks, shownTime]);

  const currentDay = lastMessage?.CurrentDay;

  return (
    <>
      <Box
        onClick={() => setModalWorldOpen(true)}
        className="fixed z-50"
        sx={{
          ...(!showSeedMap
            ? { left: "3rem", top: "1.35rem" }
            : { right: "6rem", top: "0.75rem" }),
          backgroundColor: "rgba(255, 255, 255, 1)",
          minWidth: "160px",
          height: "33px",
          borderRadius: "0.2rem",
          boxShadow: "3px 3px 2px rgba(0, 0, 0, 0.2)",
          color: "black",
          fontFamily:
            "-apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif",
          cursor: "pointer",
        }}
      >
        <Box className="flex flex-col gap-2 overflow-y-auto max-h-[200px]">
          <Box
            className="text-sm"
            sx={{
              fontSize: "16px",
              paddingBlock: "0.2rem",
              paddingInline: "0.5rem",
            }}
          >
            <Box className="flex flex-row" sx={{ gap: "0.5rem" }}>
              <Box
                sx={{
                  background: `linear-gradient(to top, ${timeColor.color1}, ${timeColor.color2})`,
                  marginTop: "0.1rem",
                  textAlign: "center",
                }}
              >
                {`${isDayIcon}`}
              </Box>
              {`Day ${currentDay} | ${minecraftTicksToTime(shownTime)}`}
            </Box>
          </Box>
        </Box>
      </Box>
      <WorldModal
        modalOpen={modalWorldOpen}
        handleModalClose={() => setModalWorldOpen(false)}
      />
    </>
  );
};

export default WorldClock;
