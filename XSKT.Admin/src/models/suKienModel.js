const toJson = (item) => {
    return {
        _id: item._id,
        name:item.name,
        content:item.content
    }
}

const fromJson = (item) => {
    return {
        _id: item._id,
        name:item.name,
        content:item.content
    }
}

const baseJson = () => {
    return {
        _id: null,
        name:null,
        content:null
    }
}

const toListModel = (items) =>{
    if(items.length > 0){
        let data = [];
        items.map((value, index) =>{
            data.push(fromJson(value));
        })
        return data??[];
    }
    return [];
}
export const suKienModel = {
    toJson, fromJson, baseJson, toListModel
}
