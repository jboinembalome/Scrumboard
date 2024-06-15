import { ColourDto } from "app/swagger";

export class BlouppyUtils {
    /**
     * Filter array by string
     *
     * @param mainArr
     * @param searchText
     * @returns {any}
     */
    public static filterArrayByString(mainArr, searchText): any {
        if (searchText === '') {
            return mainArr;
        }

        searchText = searchText.toLowerCase();

        return mainArr.filter(itemObj => {
            return this.searchInObj(itemObj, searchText);
        });
    }

    /**
     * Search in object
     *
     * @param itemObj
     * @param searchText
     * @returns {boolean}
     */
    public static searchInObj(itemObj, searchText): boolean {
        for (const prop in itemObj) {
            if (!itemObj.hasOwnProperty(prop)) {
                continue;
            }

            const value = itemObj[prop];

            if (typeof value === 'string') {
                if (this.searchInString(value, searchText)) {
                    return true;
                }
            }

            else if (Array.isArray(value)) {
                if (this.searchInArray(value, searchText)) {
                    return true;
                }
            }

            if (typeof value === 'object') {
                if (this.searchInObj(value, searchText)) {
                    return true;
                }
            }
        }
    }

    /**
     * Search in array
     *
     * @param arr
     * @param searchText
     * @returns {boolean}
     */
    public static searchInArray(arr, searchText): boolean {
        for (const value of arr) {
            if (typeof value === 'string') {
                if (this.searchInString(value, searchText)) {
                    return true;
                }
            }

            if (typeof value === 'object') {
                if (this.searchInObj(value, searchText)) {
                    return true;
                }
            }
        }
    }

    /**
     * Search in string
     *
     * @param value
     * @param searchText
     * @returns {any}
     */
    public static searchInString(value, searchText): any {
        return value.toLowerCase().includes(searchText);
    }

    /**
     * Generate a unique GUID
     *
     * @returns {string}
     */
    public static generateGUID(): string {
        function S4(): string {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }

        return S4() + S4();
    }

    /**
     * Toggle in array
     *
     * @param item
     * @param array
     */
    public static toggleInArray(item, array): void {
        if (array.indexOf(item) === -1) {
            array.push(item);
        }
        else {
            array.splice(array.indexOf(item), 1);
        }
    }

    /**
     * Handleize
     *
     * @param text
     * @returns {string}
     */
    public static handleize(text): string {
        return text.toString().toLowerCase()
            .replace(/\s+/g, '-')           // Replace spaces with -
            .replace(/[^\w\-]+/g, '')       // Remove all non-word chars
            .replace(/\-\-+/g, '-')         // Replace multiple - with single -
            .replace(/^-+/, '')             // Trim - from start of text
            .replace(/-+$/, '');            // Trim - from end of text
    }

    /**
     * Indicates whether the specified string is null or an empty string
     * @param str 
     * @returns 
     */
    public static isNullOrEmpty(str: string): boolean {
        return (!str || str.length === 0);
    }

    /**
     * Get the initials of first name et last name
     * @param firstName
     * @param lastName
     * @returns {string}
     */
    public static getInitials(firstName: string, lastName: string): string {
        if (this.isNullOrEmpty(firstName) || this.isNullOrEmpty(lastName))
            return '';

        return this.getInitialsByFullName(firstName + ' ' + lastName);       
    }

    /**
     * Get the initials of the full name
     * @param firstName
     * @param lastName
     * @returns {string}
     */
    public static getInitialsByFullName(fullName: string): string {
        if (this.isNullOrEmpty(fullName))
            return '';
            
        var names = fullName.split(' '),
            initials = names[0].substring(0, 1).toUpperCase();
        
        if (names.length > 1)
            initials += names[names.length - 1].substring(0, 1).toUpperCase();

        return initials;
    };


    public static formatColor(color: ColourDto): string {
        switch (color.colour) {
            case "bg-blue-800/30":
                return "Blue";
            case "bg-yellow-800/30":
              return "Yellow";
            case "bg-red-800/30":
              return "Red";
            case "bg-indigo-800/30":
                return "Indigo";
            case "bg-rose-800/30":
                return "Rose";
            case "bg-pink-800/30":
                return "Pink";
            case "bg-purple-800/30":
                return "Purple";
            case "bg-violet-800/30":
                return "Violet";
            case "bg-orange-800/30":
                return "Orange";
            case "bg-amber-800/30":
                return "Amber";
            case "bg-green-800/30":
                return "Green";
            case "bg-teal-800/30":
                return "Teal";
            case "bg-gray-800/30":
                return "Gray";
            default:
                return color.colour;
        }
    }
}