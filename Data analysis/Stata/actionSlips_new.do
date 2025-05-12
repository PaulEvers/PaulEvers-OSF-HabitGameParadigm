////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// Action slips T1 /////////////////////////////////////////
// Load Data
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_t1.csv", case(preserve)
// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)
drop if phase == "Practice"
drop if day == 2

// Total
sum rtReturn if wentBack == 1 & phase == "Test" // Total action slips
di 564 / (22 * 30) // 22 participants times 30 testing rounds

// Day 1
sum rtReturn if wentBack == 1 & phase == "Test" & day == 1 // Total action slips
di 281 / (22 * 15) // 22 participants times 15 testing rounds

// Day 3
sum rtReturn if wentBack == 1 & phase == "Test" & day == 3 // Total action slips
di 283 / (22 * 15) // 22 participants times 15 testing rounds


////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// Action slips and picked up T1 ///////////////////////////
// Load Data
clear
set more off	

import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds.csv", case(preserve)

// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)

tabulate participantId pickedUpCoin if phase == "Test"

keep if phase == "Test"

gen picked_day1 = pickedUpCoin if day == 1
gen picked_day3 = pickedUpCoin if day == 3

collapse (sum) picked_day1 picked_day3, by(participantId)

list participantId picked_day1 picked_day3
summarize picked_day1 picked_day3

drop if picked_day1 == 0 & picked_day3 == 0
summarize picked_day1
summarize picked_day3


////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// Mean Action slips T1 ///////////////////////////
// Load Data
clear
set more off	

import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_t1.csv", case(preserve)
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)
drop if phase == "Practice"
drop if day == 2

summarize hasActionSlip if hasActionSlip == 1
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)
gen hasActionSlip_count = hasActionSlip == 1  // Create binary indicator
collapse (sum) hasActionSlip_count, by(participantId day)  // Sum counts for each participantId and day
reshape wide hasActionSlip_count, i(participantId) j(day)  // Reshape data for paired t-test

sum hasActionSlip_count1
sum hasActionSlip_count3


////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// T-test Action slips T1 ///////////////////////////
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_t1.csv", case(preserve)

summarize hasActionSlip if hasActionSlip == 1
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)
drop if phase == "Practice"
drop if day == 2

gen hasActionSlip_count = hasActionSlip == 1  // Create binary indicator
collapse (sum) hasActionSlip_count, by(participantId day)  // Sum counts for each participantId and day
reshape wide hasActionSlip_count, i(participantId) j(day)  // Reshape data for paired t-test
ttest hasActionSlip_count1 == hasActionSlip_count3

////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// T-test Action slips T1 excl high RT ///////////////////////////
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_t1.csv", case(preserve)

summarize hasActionSlip if hasActionSlip == 1
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)
drop if phase == "Practice"
drop if day == 2
drop if rt > 1
gen hasActionSlip_count = hasActionSlip == 1  // Create binary indicator
collapse (sum) hasActionSlip_count, by(participantId day)  // Sum counts for each participantId and day
reshape wide hasActionSlip_count, i(participantId) j(day)  // Reshape data for paired t-test
ttest hasActionSlip_count1 == hasActionSlip_count3

////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// Action slips T8 - Descriptive ///////////////////////////
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_t8_new.csv", case(preserve)


drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)
drop if phase == "Practice"
drop if day == 2

summarize hasActionSlip if hasActionSlip == 1
summarize hasActionSlip if hasActionSlip == 1 & day == 1
summarize hasActionSlip if hasActionSlip == 1 & day == 3
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// T-test Action slips T8 ///////////////////////////
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_t8_new.csv", case(preserve)

summarize hasActionSlip if hasActionSlip == 1
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)
drop if phase == "Practice"
drop if day == 2

// mcci 9 5 4 4

gen hasActionSlip_count = hasActionSlip == 1  // Create binary indicator
collapse (sum) hasActionSlip_count, by(participantId day)  // Sum counts for each participantId and day
reshape wide hasActionSlip_count, i(participantId) j(day)  // Reshape data for paired t-test
ttest hasActionSlip_count1 == hasActionSlip_count3

////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// Actionslips histogram ///////////////////////////////////
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_t8.csv", case(preserve)
// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)

summarize hasActionSlip if hasActionSlip == 1

// T1 = 563
// T2 = 560
// T3 = 335
// T4 = 158
// T5 = 55
// T6 = 42
// T7 = 39
// T8 = 32
// T9 = 24
// T10 = 22

clear
input threshold seconds actionSlips
1 0.02 563
2 0.04 560
3 0.06 335
4 0.08 158
5 0.10 55
6 0.12 42
7 0.14 39
8 0.16 32
9 0.18 24
10 0.20 22
11 0.22 22
12 0.24 21
13 0.26 19
14 0.28 19
15 0.30 19
end


graph bar actionSlips, over(threshold, label(angle(45))) bar(1) ///
    ytitle("Number of Action Slips") ///
    title("Number of Total Action Slips per Threshold") ///
    b1title(Threshold) ///
	blabel(bar)
	
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// Actionslips histogram per day ///////////////////////////
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_t8.csv", case(preserve)
// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)

summarize hasActionSlip if hasActionSlip == 1 & day == 1
summarize hasActionSlip if hasActionSlip == 1 & day == 3

clear
input threshold seconds actionSlips_Day1 actionSlips_Day3
1 0.02 563
2 0.04 279 281
3 0.06 159 176
4 0.08 77 81
5 0.10 25 30
6 0.12 18 24
7 0.14 17 22
8 0.16 14 18
9 0.18 11 13
10 0.20 10 12
11 0.22 10 12
12 0.24 10 11
13 0.26 9 10
14 0.28 9 10
15 0.30 9 10
end

// graph bar actionSlips_Day1 actionSlips_Day3, over(threshold, label(angle(45))) bar(1) ///
//     ytitle("Number of Action Slips") ///
//     title("Number of Total Action Slips per Threshold") ///
//     b1title(Threshold) ///
// 	blabel(bar) ///
// 	legend(label(1 "Day 1") label(2 "Day 3"))

////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// Ttest action slips day 1 and 3 //////////////////////////

clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_t8.csv", case(preserve)
// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)

drop if phase == "Training"

count roundId

summarize hasActionSlip if hasActionSlip == 1 & day == 1
summarize hasActionSlip if hasActionSlip == 1 & day == 3

tabulate day if hasActionSlip == 1

gen slip_day1 = hasActionSlip if day == 1
gen slip_day3 = hasActionSlip if day == 3
collapse (sum) slip_day1 slip_day3, by(participantId)

summarize slip_day1
summarize slip_day3

swilk slip_day1
swilk slip_day3

// sktest slip_day1
// sktest slip_day3

// Not normally distributed
// ttest slip_day1 == slip_day3

// Non-parametric
signrank slip_day1 = slip_day3

return list


////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// Longest consecutive  ////////////////////////////////////
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_consecutive_longest.csv", case(preserve)
// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)

summarize largestConsecutive if day == 1 & phase == "Test"

sort largestConsecutive

graph bar (count), over(largestConsecutive, label(angle(45))) ///
    bar(1, color(blue)) ///
    title("Frequency of Largest Consecutive per Round") ///
    ytitle("Frequency") ///
	blabel(bar)

	stop
	histogram largestConsecutive if day == 1 & phase == "Training"
	histogram largestConsecutive if day == 1 & phase == "Test"
	
	histogram largestConsecutive if day == 1 & phase == "Training"
	
	histogram largestConsecutive if day == 3 & phase == "Test"
	histogram largestConsecutive if day == 3 & phase == "Test"
	
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// Longest consecutive day 1  ////////////////////////////////////
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_consecutive_longest.csv", case(preserve)
// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)
drop if day != 1

sort largestConsecutive

gen trainingConsecutive = largestConsecutive if phase == "Training"
gen testConsecutive = largestConsecutive if phase == "Test"

sort largestConsecutive

// diptest trainingConsecutive

graph bar (count) trainingConsecutive testConsecutive, ///
    over(largestConsecutive, label(angle(45))) ///
    bar(1, color(blue)) bar(2, color(red)) ///
    legend(order(1 "Training" 2 "Test")) ///
    title("Frequency of Largest Consecutive Logs (Day 1)") ///
    ylabel(, angle(0))
	

////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// Longest consecutive day 2  ////////////////////////////////////
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_consecutive_longest.csv", case(preserve)
// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)
drop if day != 2

sort largestConsecutive

gen trainingConsecutive = largestConsecutive if phase == "Training"

diptest trainingConsecutive

graph bar (count) trainingConsecutive, ///
    over(largestConsecutive, label(angle(45))) ///
    bar(1) bar(2, color(red)) ///
    legend(order(1 "Training")) ///
    title("Frequency of Largest Consecutive Logs (Day 2)") ///
    ylabel(, angle(0)) ///
	blabel(bar)
	
	
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// Longest consecutive day 3  ////////////////////////////////////
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_consecutive_longest.csv", case(preserve)
// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)
drop if day != 3

gen trainingConsecutive = largestConsecutive if phase == "Training"
gen testConsecutive = largestConsecutive if phase == "Test"

sort largestConsecutive

// diptest trainingConsecutive

kdensity trainingConsecutive, generate(density x)
sort density x
egen max_density = max(density)
list x density if density == max_density
gen between_modes = (x > m1 & x < m2)
list x density if between_modes & density == min(density)


graph bar (count) trainingConsecutive testConsecutive, ///
    over(largestConsecutive, label(angle(45))) ///
    bar(1, color(blue)) bar(2, color(red)) ///
    legend(order(1 "Training" 2 "Test")) ///
    title("Frequency of Largest Consecutive Logs (Day 3)") ///
    ylabel(, angle(0)) ///
	blabel(bar)
	