﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parallel_rbtree
{
    public class LockFreeRBTree: RBTree{
	    public int size = 0;
	    public LockFreeRBNode root;
	
	    public LockFreeRBTree() {
		    this.root = new LockFreeRBNode();
	    }
	
	    public int? search(int? value) {
		    if (root == null) {
			    return Int32.MinValue;
		    }
		    LockFreeRBNode temp = root;
		    while (temp != null && temp.getValue() >= 0) {
			    if (value < temp.getValue()) {
				    temp = temp.getLeft();
			    }else if (value > temp.getValue()) {
				    temp = temp.getRight();
			    } else{
				    return temp.getValue();
			    }
		    }
            if (temp == null)
                return null;
            else
                return temp.getValue();
		   // return temp==null?null:temp.getValue();
	    }
	
	    public void insert(int? value)
        {
            //throws NullPointerException
            if (value == null)
            {
                throw new System.ArgumentException("Parameter cannot be null", "original");
            }
            LockFreeRBNode insertedNode = new LockFreeRBNode(value);
		    LockFreeRBNode temp1, temp2;
            insertedNode.flag.Set(true);
		    while (true) {
			    temp1 = this.root;
			    temp2 = null;
			    while (temp1.getValue() >= 0) {
				    temp2 = temp1;
				    if (value < temp1.getValue()) {
					    temp1 = temp1.getLeft();
				    } else {
					    temp1 = temp1.getRight();
				    }
			    }
			    if (!setupLocalAreaForInsert(temp2)) {
                    temp2.flag.Set(false); 
                    continue;
			    } else {
				    break;
			    }
		    }
		
		    insertedNode.setParent(temp2);
		    if (temp2 == null) { 
			    this.root = insertedNode; 
		    } else if (value < temp2.getValue()) {
			    temp2.setLeft(insertedNode);
		    } else {
			    temp2.setRight(insertedNode);
		    }
		    insertedNode.getLeft().setParent(insertedNode);
		    insertedNode.getRight().setParent(insertedNode);
		    insertedNode.setRed(true);
		    rbInsertFixup(insertedNode);	
	    }
	
	    private Boolean setupLocalAreaForInsert(LockFreeRBNode x) {
		    if (x == null) {
			    return true;
		    }
		    LockFreeRBNode parent = x.getParent();
		    LockFreeRBNode uncle;
		    if (parent == null) return true;
		    if (!x.flag.CompareAndSet(false, true)) {
			    return false;
		    }
		    if (!parent.flag.CompareAndSet(false, true)) {
			    return false;
		    }
		    if (parent != x.getParent()) {
			    parent.flag.Set(false);
			    return false;
		    }
		    if (x == x.getParent().getLeft()) {
			    uncle = x.getParent().getRight();
		    } else {
			    uncle = x.getParent().getLeft();
		    }
		    if (uncle != null && !uncle.flag.CompareAndSet(false, true)) {
			    x.getParent().flag.Set(false);
			    return false;
		    }
		    return true;
	    }
	
	    private void rbInsertFixup(LockFreeRBNode x) {
		    LockFreeRBNode temp, parent, uncle = null, gradparent = null;
		    parent = x.getParent();
		    List<LockFreeRBNode> local_area = new List<LockFreeRBNode>();
		    local_area.Add(x);
		    local_area.Add(parent);
		
		    if (parent != null) {
			    gradparent = parent.getParent();	
		    }
		
		    if (gradparent != null) {
			    if (gradparent.getLeft() == parent) {
				    uncle = gradparent.getRight();
			    } else {
				    uncle = gradparent.getLeft();		
			    }
		    }

		    local_area.Add(uncle);
		    local_area.Add(gradparent);

		    while (x.getParent()!= null && x.getParent().IsRed()) {
			    parent = x.getParent();
			    gradparent = gradparent.getParent();
			
			    if (x.getParent() == x.getParent().getParent().getLeft()) {
				    temp = x.getParent().getParent().getRight();
				    uncle = temp;
				    local_area.Add(x);
				    local_area.Add(parent);
				    local_area.Add(gradparent);
				    local_area.Add(uncle);
				
				    if (temp.IsRed()) {
					    x.getParent().setRed(false);
					    temp.setRed(false);
					    x.getParent().getParent().setRed(true);
					    x = moveLocalAreaUpward(x, local_area);
				    } else {
					    if (x == x.getParent().getRight()) {
						    // Case 2
						    x = x.getParent();
						    leftRotate(x);
					    }
					    // Case 3
					    x.getParent().setRed(false);
					    x.getParent().getParent().setRed(true);
					    rightRotate(x.getParent().getParent());
				    }
			    } else {
				    temp = x.getParent().getParent().getLeft();
				    uncle = temp;
				
				    local_area.Add(x);
				    local_area.Add(parent);
				    local_area.Add(gradparent);
				    local_area.Add(uncle);
				
				    if (temp.IsRed()) {
					    // Case 1
					    x.getParent().setRed(false);
					    temp.setRed(false);
					    x.getParent().getParent().setRed(true);
					    x = moveLocalAreaUpward(x, local_area);
				    } else {
					    if (x == x.getParent().getLeft()) {
						    // Case 2
						    x = x.getParent();
						    rightRotate(x);
					    }
					    // Case 3
					    x.getParent().setRed(false);
					    x.getParent().getParent().setRed(true);
					    leftRotate(x.getParent().getParent());
				    }
			    }
		    }
		
		    this.root.setRed(false);

		    foreach (LockFreeRBNode node in local_area) {
                if (node != null) node.flag.Set(false);
		    }
	    }
	
	    private LockFreeRBNode moveLocalAreaUpward(LockFreeRBNode x, List<LockFreeRBNode> working) {
		    LockFreeRBNode parent = x.getParent();
		    LockFreeRBNode grandparent = parent.getParent();
		    LockFreeRBNode uncle;
		    if (parent == grandparent.getLeft()){
			    uncle = grandparent.getRight();
		    } else {
			    uncle = grandparent.getLeft();
		    }
		
		    LockFreeRBNode updated_x, updated_parent = null, updated_uncle = null, updated_grandparent = null;
		    updated_x = grandparent;
		
		    while (true && updated_x.getParent()!= null) {
			    updated_parent = updated_x.getParent();
			    if (!updated_parent.flag.CompareAndSet(false, true)) {
				    continue;
			    }
			    updated_grandparent = updated_parent.getParent();
			    if (updated_grandparent == null) break;
			    if (!updated_grandparent.flag.CompareAndSet(false, true)) {
                    updated_parent.flag.Set(false);
                    continue;
			    }
			    if (updated_parent == updated_grandparent.getLeft()) {
				    updated_uncle = updated_grandparent.getRight();
			    } else {
				    updated_uncle = updated_grandparent.getLeft();
			    }
			
			    if (updated_uncle != null && !updated_uncle.flag.CompareAndSet(false, true)) {
                    updated_grandparent.flag.Set(false);
                    updated_parent.flag.Set(false);
				    continue;
			    }
			    break;
		    }
		
		    working.Add(updated_x);
		    working.Add(updated_parent);
		    working.Add(updated_grandparent);
		    working.Add(updated_uncle);
		
		    return updated_x;	
	    }
	
	    private  void leftRotate(LockFreeRBNode x) {
		    if (x == null) return;
		    LockFreeRBNode y = x.getRight();
		    x.setRight(y.getLeft());
		    if (y.getLeft() != null) {
			    y.getLeft().setParent(x);
		    }
		    y.setParent(x.getParent());
		    if (x.getParent() == null) this.root = y;
		    else{
			    if (x == x.getParent().getLeft())
				    x.getParent().setLeft(y);
			    else
				    x.getParent().setRight(y);
		    }
		    y.setLeft(x);
		    x.setParent(y);
	    }
	
	    private void rightRotate(LockFreeRBNode y) {
		    if (y == null) return;
		    LockFreeRBNode x = y.getLeft();
		    y.setLeft(x.getRight());
		    if (x.getRight() != null) {
			    x.getRight().setParent(y);
		    }
		    x.setParent(y.getParent());
		    if (y.getParent() == null) this.root = x;
		    else{
			    if (y == y.getParent().getLeft())
				    y.getParent().setLeft(x);
			    else
				    y.getParent().setRight(x);
		    }
		    x.setRight(y);
		    y.setParent(x);
	    }
	
	
	    public int getheight(LockFreeRBNode root) {
		    if (root == null)
			    return 0;
		    return Math.Max(getheight(root.getLeft()), getheight(root.getRight())) + 1;
	    }


	    public void preOrder(LockFreeRBNode n ){
		
		    if (n == null)
			    return;
		    n.displayNode();
		    preOrder(n.getLeft());
		    preOrder(n.getRight());
	    }

	    public void breadth(LockFreeRBNode n ){
		
		    if (n == null)
			    return;
		    n.displayNode();
		    preOrder(n.getLeft());
		    preOrder(n.getRight());
	    }
	
    } 

}
