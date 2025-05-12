#utils::install.packages("lme4", type = "source")
#install.packages("lmerTest")
#install.packages( "stargazer")

# Load the package
library(lmerTest)
library(tidyverse)
library(lme4)
library(emmeans)
library(stargazer)

# Import the CSV file
data <- read.csv("C:/Users/paulu/OneDrive - TU Eindhoven/TUe/Thesis/Data/rounds_t8_new.csv", stringsAsFactors = FALSE)

# Exclude outliers
data <- data %>% filter(!participantId %in% c(1, 4, 12, 17, 20, 21, 22, 23, 28, 36, 38, 40))

# Drop practice phase
data <- data %>% filter(phase != "Practice")

# Drop if day == 2
data <- data %>% filter(day != 2)

# Only keep correct responses for each phase
data <- data %>% filter(!(is.na(rtReturn) & phase == "Training"))
data <- data %>% filter(!(is.na(rtForward) & phase == "Test"))

# Modify totalRounds
data <- data %>% mutate(totalRounds = totalRounds - 5)

# Apply additional filters on totalRounds
data <- data %>% filter(!(phase == "Training" & day == 1 & totalRounds <= 45))
data <- data %>% filter(!(phase == "Training" & day == 3 & totalRounds > 75 & totalRounds <= 180))

# Convert phase to a factor
#data <- data %>% mutate(phase_num = as.factor(phase))

data$phase <- factor(data$phase, levels = c("Training", "Test"))
data$day <- factor(data$day, levels = c(1, 3))

contrasts(data$phase) <- contr.sum(2)
contrasts(data$day) <- contr.sum(2)

# Mixed effects model
model.lmer2 <- lmer(rt ~ phase * day + (1 | participantId), data = data)

summary(model.lmer2)

#stargazer(model.lmer2, type = "text",
#          digits = 3,
#          star.cutoffs = c(0.05, 0.01, 0.001),
#         digit.separator = "")

# Marginal means
margins_result <- emmeans(model.lmer2, ~ phase | day)
print(margins_result)
