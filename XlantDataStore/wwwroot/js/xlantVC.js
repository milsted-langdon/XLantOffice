function vcLink(oldText){
	var array = new Array();
	array = oldText.match(/VC[0-9]+/g);
	var newText = oldText;
	if (array == null){
		return oldText;
	}
	else{
		for(i = 0; i< array.length; i++){
			var id = array[i].substring(2,array[i].length);
			newText = newText + " - <a href=vclink://" + id + "/1/1>View</a>";
		}
		return newText;
	}
}