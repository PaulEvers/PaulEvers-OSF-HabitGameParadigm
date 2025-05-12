////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// Returning times ////////////////////////////
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_t8.csv", case(preserve)
// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)

tabulate wentBack phase if day == 1
tabulate wentBack phase if day == 2
tabulate wentBack phase if day == 3

////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// Scatterplot t8 ////////////////////////////
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_t8.csv", case(preserve)
// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)
drop if phase == "Practice"	
drop if wentBack != 1

replace totalRounds = totalRounds - 5
encode phase, gen(phase_num)
regress rt totalRounds i.phase_num

// drop if rt > 2

summarize rt if day == 1 & phase == "Training"
summarize rt if day == 1 & phase == "Test"

summarize rt if day == 2 & phase == "Training"

summarize rt if day == 3 & phase == "Training"
summarize rt if day == 3 & phase == "Test"

twoway (scatter rt totalRounds if phase == "Training", mcolor(%30)) ///
       (scatter rt totalRounds if phase == "Test", mcolor(%30 red)), ///
       xlabel(, grid) ylabel(, grid) ///
       xtitle("Round") ytitle("RT (s)") ///
       title("RT over Total Rounds") ///
       legend(off)
	   
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////// Means /////////////////////////////////////
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_t8_new.csv", case(preserve)
// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)
drop if phase == "Practice"	
drop if wentBack != 1

replace totalRounds = totalRounds - 5

summarize rt if day == 1 & phase == "Training"
summarize rt if day == 1 & phase == "Test"

summarize rt if day == 2 & phase == "Training"

summarize rt if day == 3 & phase == "Training"
summarize rt if day == 3 & phase == "Test"


////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
///////////////////// Means - Optimal choice ///////////////////////
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_t8_new.csv", case(preserve)
// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)
drop if phase == "Practice"
drop if day == 2

// Only keep the correct responses for each phase
drop if missing(rtReturn) & phase == "Training"
drop if missing(rtForward) & phase == "Test"

replace totalRounds = totalRounds - 5

drop if phase == "Training" & day == 1 & totalRounds <= 45 
drop if phase == "Training" & day == 3 & totalRounds > 75 & totalRounds <= 180 

summarize rt
summarize rt if day == 1
summarize rt if day == 3

summarize rt if day == 1 & phase == "Training"
summarize rt if day == 1 & phase == "Test"

summarize rt if day == 3 & phase == "Training"
summarize rt if day == 3 & phase == "Test"

////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
///////////////////// With forward - RT Switch cost optimal choice +-15 rounds///////////////////////
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_t8_new.csv", case(preserve)
// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)
drop if phase == "Practice"
drop if day == 2

// Only keep the correct responses for each phase
drop if missing(rtReturn) & phase == "Training"
drop if missing(rtForward) & phase == "Test"

replace totalRounds = totalRounds - 5

drop if phase == "Training" & day == 1 & totalRounds <= 45 
drop if phase == "Training" & day == 3 & totalRounds > 75 & totalRounds <= 180 

encode phase, gen(phase_num)

// mixed rt i.phase_num##i.day || participantId:
// margins phase_num#day

gen rttraining = rt if phase == "Training"
gen rttest = rt if phase == "Test"

label define day_label 1 "Day 1" 3 "Day 3"
label values day day_label

graph bar (mean) rttraining rttest, over(day) ///
    legend(label(1 "Training") label(2 "Test")) ///
    ytitle("Reaction Time (s)") ///
	title("Mean Reaction Time by Day and Phase") ///
    blabel(bar, format(%9.2f))


////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
///////////////////// With forward - RT Switch cost optimal choice +-15 rounds - Covariate ///////////////////////
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_t8_new.csv", case(preserve)
// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)
drop if phase == "Practice"
drop if day == 2

// Only keep the correct responses for each phase
drop if missing(rtReturn) & phase == "Training"
drop if missing(rtForward) & phase == "Test"

replace totalRounds = totalRounds - 5

drop if phase == "Training" & day == 1 & totalRounds <= 45 
drop if phase == "Training" & day == 3 & totalRounds > 75 & totalRounds <= 180 

// Deviation coding
// generate rtTraining = rt if phase == "Training"
// generate rtTest = rt if phase == "Test"

collapse (mean) rt, by(participantId day phase)
reshape wide rt, i(participantId day) j(phase) string


mixed rtTest day rtTraining   || participantId:
anova rtTest participantId day c.rtTraining, repeated(day)

////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
///////////////////// With forward - Switch Cost SRBAI correlation +-15 rounds ///////////////////////
clear
set more off

use "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\Do-files\switchcosts_day3.dta", clear

merge 1:1 participantId using "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\Do-files\srbai_day3.dta"
drop _merge

// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)

gen score = (srbai1 + srbai2 + srbai3 + srbai4) / 4
sort participantId

pwcorr rtSwitchCost score, sig
twoway (scatter score rtSwitchCost) (lfit score rtSwitchCost)


////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
///////////////////// With forward - Switch Cost SRBAI correlation +-15 rounds ///////////////////////

// Transform SRBAI //
clear
set more off	

import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\habitSurveyResults.csv", case(preserve)
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)

gen score = (srbai1 + srbai2 + srbai3 + srbai4) / 4
sort participantId
collapse (mean) score, by(participantId)
/////////////////////////////////////////////////////////////////////////////////

// Transform Switch Costs //
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_t8_new.csv", case(preserve)
// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)
drop if phase == "Practice"
drop if day == 2

// Only keep the correct responses for each phase
drop if missing(rtReturn) & phase == "Training"
drop if missing(rtForward) & phase == "Test"

replace totalRounds = totalRounds - 5

drop if phase == "Training" & day == 1 & totalRounds <= 45 
drop if phase == "Training" & day == 3 & totalRounds > 75 & totalRounds <= 180 

collapse (mean) rtReturn rtForward, by(participantId)
gen rtSwitchCost = rtForward - rtReturn
//////////////////////////////////////////////////////////////////////////////////
clear
set more off	
use "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\Do-files\switchcosts_mean.dta", clear

merge 1:1 participantId using "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\Do-files\srbai_mean.dta"
drop _merge 

pwcorr rtSwitchCost score, sig
twoway (scatter score rtSwitchCost) (lfit score rtSwitchCost)


////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
///////////////////// With forward - Switch Cost SRBAI correlation +-15 rounds id + day///////////////////////

// Transform SRBAI //
clear
set more off	

import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\habitSurveyResults.csv", case(preserve)
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)

gen score = (srbai1 + srbai2 + srbai3 + srbai4) / 4
drop srbai1 srbai2 srbai3  srbai4
drop if day == 2
sort participantId
/////////////////////////////////////////////////////////////////////////////////
clear
set more off	
import delimited "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\rounds_t8_new.csv", case(preserve)
// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)
drop if phase == "Practice"
drop if day == 2

// Only keep the correct responses for each phase
drop if missing(rtReturn) & phase == "Training"
drop if missing(rtForward) & phase == "Test"

replace totalRounds = totalRounds - 5

drop if phase == "Training" & day == 1 & totalRounds <= 45 
drop if phase == "Training" & day == 3 & totalRounds > 75 & totalRounds <= 180 

//////////////////
clear
set more off
use "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\Do-files\rounds_t8.dta", clear
 
merge m:1 participantId day using "C:\Users\paulu\OneDrive - TU Eindhoven\TUe\Thesis\Data\Do-files\srbai_day1-3.dta"

// Drop outliers
drop if (participantId == 26 | participantId == 9) & day == 3


egen pair_id = group(participantId day)

collapse (mean) rtReturn rtForward score day, by(pair_id)
gen rtSwitchCost = rtForward - rtReturn
drop if rtSwitchCost == .

pwcorr rtSwitchCost score, sig
twoway ///
    (scatter score rtSwitchCost if day == 1, ///
        legend(label(1 "Day 1"))) ///
    (scatter score rtSwitchCost if day == 3, mcolor(green) legend(label(2 "Day 3"))) ///
    (lfit score rtSwitchCost, lcolor(red)), ///
    ytitle("SRBAI Score") ///
    xtitle("RT Switch Cost (s)") ///
    title("Correlation RT Switch Cost and SRBAI Score excl. Outliers") ///
    ylabel(0(1)7) ///
    yscale(range(0 7))