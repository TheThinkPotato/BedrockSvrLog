import React from "react";
import {
  Dialog,
  DialogContent,
  DialogTitle,
  IconButton,
  Typography,
  Box,
  Divider,
} from "@mui/material";
import { Close as CloseIcon } from "@mui/icons-material";
import { useWorldData } from "../../../context/WorldContext";

interface NewspaperModalProps {
  open: boolean;
  onClose: () => void;
}

const NewspaperModal: React.FC<NewspaperModalProps> = ({ open, onClose }) => {
  const { lastMessage: world } = useWorldData();

  return (
    <Dialog
      open={open}
      onClose={onClose}
      maxWidth="lg"
      fullWidth
      slotProps={{
        paper: {
          sx: {
          backgroundColor: "#f5f5dc", // Cream/off-white newspaper color
            borderRadius: 0,
            boxShadow: "0 8px 32px rgba(0,0,0,0.3)",
          },
        },
      }}
    >
      <DialogTitle
        sx={{
          backgroundColor: "#2c3e50",
          color: "white",
          textAlign: "center",
          position: "relative",
          padding: "16px 24px",
          fontFamily: "serif",
        }}
      >
        <Typography
          variant="h3"
          component="h1"
          sx={{
            fontFamily: "serif",
            fontWeight: "bold",
            fontSize: "2.5rem",
            letterSpacing: "2px",
            textShadow: "2px 2px 4px rgba(0,0,0,0.5)",
          }}
        >
          THE {world?.Name.toUpperCase()} TIMES
        </Typography>
        <Typography
          variant="subtitle1"
          sx={{
            fontFamily: "serif",
            fontSize: "1rem",
            marginTop: "8px",
            opacity: 0.9,
          }}
        >
          EST. 2024 • VOL. 1, NO. 1
        </Typography>
        <IconButton
          onClick={onClose}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: "white",
            "&:hover": {
              backgroundColor: "rgba(255,255,255,0.1)",
            },
          }}
        >
          <CloseIcon />
        </IconButton>
      </DialogTitle>

      <DialogContent sx={{ padding: 0, backgroundColor: "#f5f5dc" }}>
        <Box sx={{ padding: "24px", fontFamily: "serif", color: "black" }}>
          {/* Header with date and price */}
          <Box
            sx={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              marginBottom: "16px",
              borderBottom: "2px solid #333",
              paddingBottom: "8px",
            }}
          >
            <Typography
              variant="body2"
              sx={{ fontFamily: "serif", fontSize: "0.9rem" }}
            >
              {new Date().toLocaleDateString("en-US", {
                weekday: "long",
                year: "numeric",
                month: "long",
                day: "numeric",
              })}
            </Typography>
            <Typography
              variant="body2"
              sx={{ fontFamily: "serif", fontSize: "0.9rem" }}
            >
              PRICE: 1 EMERALD
            </Typography>
          </Box>

          {/* Main headline */}
          <Box sx={{ marginBottom: "24px" }}>
            <Typography
              variant="h2"
              sx={{
                fontFamily: "serif",
                fontWeight: "bold",
                fontSize: "2.2rem",
                lineHeight: 1.2,
                textTransform: "uppercase",
                letterSpacing: "1px",
                color: "#2c3e50",
                marginBottom: "8px",
              }}
            >
              BREAKING: New Server Features
            </Typography>
            <Typography
              variant="h6"
              sx={{
                fontFamily: "serif",
                fontSize: "1.1rem",
                color: "#555",
                fontStyle: "italic",
                marginBottom: "16px",
              }}
            >
              Groundbreaking updates revolutionize the gaming experience
            </Typography>
          </Box>

          {/* Two column layout */}
          <Box sx={{ display: "flex", gap: "24px" }}>
            {/* Left column */}
            <Box sx={{ flex: 1 }}>
              <Typography
                variant="body1"
                sx={{
                  fontFamily: "serif",
                  fontSize: "1rem",
                  lineHeight: 1.6,
                  textAlign: "justify",
                  marginBottom: "16px",
                }}
              >
                In a historic development that has sent shockwaves through the
                gaming community, our development team has announced a
                comprehensive suite of new features that promise to transform
                the server experience. The updates include advanced player
                statistics, real-time world mapping, and enhanced user interface
                components.
              </Typography>

              <Typography
                variant="body1"
                sx={{
                  fontFamily: "serif",
                  fontSize: "1rem",
                  lineHeight: 1.6,
                  textAlign: "justify",
                  marginBottom: "16px",
                }}
              >
                "This is a game-changer," said the lead developer in an
                exclusive interview. "We've been working tirelessly to create
                tools that not only enhance gameplay but also provide valuable
                insights into player behavior and world dynamics."
              </Typography>

              <Divider sx={{ margin: "16px 0", borderColor: "#333" }} />

              <Typography
                variant="h4"
                sx={{
                  fontFamily: "serif",
                  fontWeight: "bold",
                  fontSize: "1.4rem",
                  color: "#2c3e50",
                  marginBottom: "12px",
                }}
              >
                Player Statistics Dashboard
              </Typography>

              <Typography
                variant="body1"
                sx={{
                  fontFamily: "serif",
                  fontSize: "1rem",
                  lineHeight: 1.6,
                  textAlign: "justify",
                }}
              >
                The new statistics system provides detailed insights into player
                performance, including kill counts, world exploration data, and
                activity patterns. This feature has been met with overwhelming
                positive feedback from the community.
              </Typography>
            </Box>

            {/* Right column */}
            <Box sx={{ flex: 1 }}>
              <Typography
                variant="h4"
                sx={{
                  fontFamily: "serif",
                  fontWeight: "bold",
                  fontSize: "1.4rem",
                  color: "#2c3e50",
                  marginBottom: "12px",
                }}
              >
                World Mapping Technology
              </Typography>

              <Typography
                variant="body1"
                sx={{
                  fontFamily: "serif",
                  fontSize: "1rem",
                  lineHeight: 1.6,
                  textAlign: "justify",
                  marginBottom: "16px",
                }}
              >
                Revolutionary seed-based mapping technology now allows players
                to visualize their world in unprecedented detail. The
                interactive map provides real-time updates and comprehensive
                terrain analysis.
              </Typography>

              <Divider sx={{ margin: "16px 0", borderColor: "#333" }} />

              <Typography
                variant="h4"
                sx={{
                  fontFamily: "serif",
                  fontWeight: "bold",
                  fontSize: "1.4rem",
                  color: "#2c3e50",
                  marginBottom: "12px",
                }}
              >
                Community Response
              </Typography>

              <Typography
                variant="body1"
                sx={{
                  fontFamily: "serif",
                  fontSize: "1rem",
                  lineHeight: 1.6,
                  textAlign: "justify",
                }}
              >
                Early adopters have praised the intuitive design and powerful
                functionality. "It's like having a command center for your
                gaming experience," remarked one enthusiastic player. The
                community eagerly anticipates future updates.
              </Typography>
            </Box>
          </Box>

          {/* Footer */}
          <Box
            sx={{
              marginTop: "32px",
              paddingTop: "16px",
              borderTop: "2px solid #333",
              textAlign: "center",
            }}
          >
            <Typography
              variant="body2"
              sx={{
                fontFamily: "serif",
                fontSize: "0.8rem",
                color: "#666",
              }}
            >
              "All the News That's Fit to Print" • Established 2024
            </Typography>
          </Box>
        </Box>
      </DialogContent>
    </Dialog>
  );
};

export default NewspaperModal;