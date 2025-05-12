////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// Graph  ///////////////////////////////////
clear
set more off	
// import delimited "\rounds_roundLogs_merge_new.csv", case(preserve)
// merge m:1 roundId using "\rounds_t8.dta", keepusing(wentBack)
use "\rounds_t8_logs_wentback.dta", clear

// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)

drop if wentBack == 1
drop if pickedUpCoin == 1		

drop if t < coinSpawnTime - 2
drop if t > coinSpawnTime + 5

// Example graph stagnation	
graph twoway ///
    (line d t if roundId == 208, lcolor(blue) lwidth(medium) leg(off)) ///
    (pcarrowi 100 9.22 100 10.04, lcolor(black) mcolor(black) leg(off)) ///
    , ///
    title("Example Round Stagnation") ///
    xlabel(, grid) ylabel(, grid) ///
    xtitle("Time (s)") ytitle("Distance to Finish") ///
    xline(9.22, lcolor(red) lpattern(dash) ) ///
    xline(10.04, lcolor(red) lpattern(dash))
	
// Example graph ignoring	
graph twoway ///
    (line d t if roundId == 192, lcolor(blue) lwidth(medium) leg(off)) ///
    , ///
    title("Example Round Continuing") ///
    xlabel(, grid) ylabel(, grid) ///
    xtitle("Time (s)") ytitle("Distance to Finish") ///
    xline(7.94, lcolor(red) lpattern(dash) )
	
stop		
levelsof roundId, local(rounds)
foreach r in `rounds' {
    // Get the coinSpawnTime for the current roundId
    su coinSpawnTime if roundId == `r'
    local spawnTime = r(mean)
	
    graph twoway line d t if roundId == `r', ///
        title("Distance over Time for Round `r'") ///
        xlabel(, grid) ylabel(, grid) ///
        xtitle("Time (s)") ytitle("Distance to Finish") ///
        lcolor(blue) lwidth(medium) ///
        xline(`spawnTime', lcolor(red) lpattern(dash))

    graph export "\Do-files\Cleaned\Graphs\distance_vs_time_roundId_`r'.png", replace
	graph close
}

////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// Example Graph Training ///////////////////////////////////
clear
set more off	
import delimited "\rounds_roundLogs_merge_new.csv", case(preserve)

// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)	
drop if pickedUpCoin == 0
drop if phase != "Training"


drop if t < coinSpawnTime - 1
drop if t > coinSpawnTime + 3

// Example graph		
graph twoway ///
    (line d t if roundId == 198, lcolor(blue) lwidth(medium) leg(off)) ///
    (pcarrowi 70 6.98 70 7.18, lcolor(black) mcolor(black) leg(off)) ///
    , ///
    title("Distance over Time for Round 198") ///
    xlabel(, grid) ylabel(, grid) ///
    xtitle("Time (s)") ytitle("Distance to Finish") ///
    xline(6.98, lcolor(red) lpattern(dash) ) ///
    xline(7.18, lcolor(red) lpattern(dash))

	////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
////////////////////// Example Graph Training ///////////////////////////////////
clear
set more off	
import delimited "\rounds_roundLogs_merge_new.csv", case(preserve)

// Exclude outliers
drop if inlist(participantId, 1,4,12,17,20,21,22,23,28,36,38,40)	
drop if pickedUpCoin == 0
drop if phase != "Training"

// Example graph		
graph twoway ///
    (line d t if roundId == 242, lcolor(blue) lwidth(medium) leg(off)) ///
    , ///
    title("Distance over Time for Round 242") ///
    xlabel(, grid) ylabel(, grid) ///
    xtitle("Time (s)") ytitle("Distance to Finish")

